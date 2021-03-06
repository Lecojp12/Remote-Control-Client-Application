﻿// Copyright © LECO Corporation 2009-2013.  All Rights Reserved.

using System;
using System.ComponentModel;

namespace CornerstoneRemoteControlClient.ViewModels
{
    public class RemoteControlDataViewModel : CommandDataViewModel
    {
        public RemoteControlDataViewModel(IConnectionViewModel connectionViewModel)
            : base(connectionViewModel, "Remote Control Commands")
        {
            InitializeCommands();
            SetupAvailability();
        }

        public override Boolean CommandsEnabled
        {
            get { return ConnectionViewModel.SupportsRemoteControl && ConnectionViewModel.InRemoteControlMode; }
        }

        protected override void ConnectionViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.ConnectionViewModelPropertyChanged(sender, e);

            SetupAvailability();
        }

        private void SetupAvailability()
        {
            if (ConnectionViewModel.Connected)
            {
                if (ConnectionViewModel.LoggedOn)
                {
                    if (ConnectionViewModel.InRemoteControlMode)
                    {
                        if (CommandsEnabled)
                            AvailabilityText = "The connected instrument supports Remote Control commands.";
                        else
                            AvailabilityText = "The connected instrument does not support Remote Control commands.";
                    }
                    else
                    {
                        AvailabilityText = "Remote Control commands cannot be executed because the connected instrument is not currently in remote control mode.";
                    }
                }
                else
                {
                    AvailabilityText = "In order to execute Remote Control commands use the log on controls to specify a valid user.";
                }
            }
            else
            {
                AvailabilityText = "In order to execute Remote Control commands use the connection controls to establish a connection to a Cornerstone instrument.";
            }
        }

        private void InitializeCommands()
        {
            using (Commands.AcquireLock())
            {
                String name;
                String description;

                {
                    name = "Abort";
                    description = "Aborts a sequence currently running on the instrument.";
                    var command = new ParameterlessCommandViewModel(name, description);
                    Commands.Add(command);
                }
                {
                    name = "Analyze";
                    description = "Starts the analysis sequence.";
                    var command = new ParameterlessCommandViewModel(name, description);
                    Commands.Add(command);
                }
                {
                    name = "AssignNextToAnalyze";
                    description = "Specifies the next replicate to analyze. If the Tag attribute is omitted or the value left at the default (0), then the first unanalyzed replicate within the specified set will be marked as next to analyze.";
                    var command = new AttributeParameteredCommandViewModel(name, description);
                    command.AddParameter("SetKey", "The unique key that identifies the set. Leading zeros may be omitted.", "0");
                    command.AddParameter("Tag", "The identifier of the replicate within the set.", "0");
                    Commands.Add(command);
                }
                {
                    name = "ContinueAnalysis";
                    description = "Continues the analysis sequence when Cornerstone has prompted the user to perform an action and indicate when analysis should continue.";
                    var command = new ParameterlessCommandViewModel(name, description);
                    Commands.Add(command);
                }
                {
                    name = "DeleteSamples";
                    description = "Attempts to delete the specified sets and replicates. Sets and replicates must first be marked as \"Excluded\" before being deleted.";
                    var command = new SetAndRepsCommandViewModel(name, description);
                    Commands.Add(command);
                }
                {
                    name = "ExcludeSamples";
                    description = "Sets the state of the specified sets and replicates to be excluded.";
                    var command = new SetAndRepsCommandViewModel(name, description);
                    Commands.Add(command);
                }
                {
                    name = "IncludeSamples";
                    description = "Sets the state of the specified sets and replicates to be included.";
                    var command = new SetAndRepsCommandViewModel(name, description);
                    Commands.Add(command);
                }
                {
                    name = "ModifySamples";
                    description = "Modifies the values of fields on sets and replicates.";
                    var command = new ModifySamplesCommandViewModel(name, description);
                    Commands.Add(command);
                }
                {
                    name = "PauseSamples";
                    description = "Modifies the paused state of the specified replicates.";
                    var command = new PauseSamplesCommandViewModel(name, description);
                    Commands.Add(command);
                }
                {
                    name = "PerformInstrumentAction";
                    description = "Performs the specified action on the instrument.";
                    var command = new AttributeParameteredCommandViewModel(name, description);
                    command.AddParameter("Action", "Indicates the action to perform on the instrument. If this parameter is not provided or the value is blank, the list of available actions is returned.", "");
                    Commands.Add(command);
                }
                {
                    name = "RecalcSamples";
                    description = "Performs a recalculation on the specified sets and replicates.";
                    var command = new SetAndRepsCommandViewModel(name, description);
                    Commands.Add(command);
                }
                {
                    name = "SetGasState";
                    description = "Sets the gas state on the Cornerstone instrument to the specified state.";
                    var command = new AttributeParameteredCommandViewModel(name, description);
                    command.AddParameter("State", "Gas state to set on instrument. Valid values are \"ON\", \"OFF\", and \"CONSERVE\".", "");
                    Commands.Add(command);
                }
                {
                    name = "TransmitSamples";
                    description = "Transmits the specified set and replicate data using the specified transport.";
                    var command = new TransmitSetAndRepsCommandViewModel(name, description);
                    command.AddParameter("Key", "The unique key that identifies the transport. Leading zeros may be omitted.", "0");
                    Commands.Add(command);
                }
            }
        }
    }
}