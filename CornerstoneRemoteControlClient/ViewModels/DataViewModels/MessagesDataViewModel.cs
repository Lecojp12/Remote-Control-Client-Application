﻿// Copyright © LECO Corporation 2013.  All Rights Reserved.

using System;
using System.Windows;
using System.Xml.Linq;
using CornerstoneRemoteControlClient.Events;
using CornerstoneRemoteControlClient.Helpers;

namespace CornerstoneRemoteControlClient.ViewModels.DataViewModels
{
    /// <summary>
    /// View model behind the messages page.
    /// </summary>
    public class MessagesDataViewModel : DataViewModel
    {
        #region Constructor

        public MessagesDataViewModel()
            : base("Messages")
        {
            Messages = new ObservableList<string>(Application.Current.Dispatcher);

            EventAggregatorContext.Current.GetEvent<MessageDataEvent>().Subscribe(OnMessageDataReceived);
        }

        #endregion

        #region Private Methods

        private void OnMessageDataReceived(XDocument messageDoc)
        {
            if (messageDoc != null && messageDoc.Root != null)
            {
                using (Messages.AcquireLock())
                {
                    Messages.Add(messageDoc.ToString());
                }
            }

            if (!IsSelected)
            {
                IsFlashing = true;
            }
        }

        #endregion

        #region Public Properties

        public ObservableList<String> Messages { get; private set; }

        #endregion
    }
}
