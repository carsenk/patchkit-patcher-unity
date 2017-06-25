﻿using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using PatchKit.Api.Models.Main;
using PatchKit.Patcher.AppData.Remote;
using PatchKit.Patcher.AppUpdater.Commands;
using PatchKit.Patcher.Unity.UI.Dialogs;
using PatchKit.Unity.Patcher.Cancellation;
using PatchKit.Unity.Patcher.Status;
using UnityEngine;

class ValidateLicenseCommandTest
{
    [Test]
    public void Execute_UsesCachedKey()
    {
        var cache = new MockCache();

        const string key = "this-key-should-be-cached";

        for (int i = 0; i < 2; i++)
        {
            var licenseDialog = Substitute.For<ILicenseDialog>();
            licenseDialog.Display(Arg.Any<LicenseDialogMessageType>()).ReturnsForAnyArgs(new LicenseDialogResult()
            {
                Key = key,
                Type = LicenseDialogResultType.Confirmed
            });

            var remoteMetaData = Substitute.For<IRemoteMetaData>();
            remoteMetaData.GetAppInfo().Returns(new App()
            {
                UseKeys = true
            });

            var statusMonitor = Substitute.For<IStatusMonitor>();

            var command = new ValidateLicenseCommand(licenseDialog, remoteMetaData, cache);
            command.Prepare(statusMonitor);
            command.Execute(CancellationToken.Empty);

            if (i == 0)
            {
                licenseDialog.Received(1).Display(Arg.Any<LicenseDialogMessageType>());
            }
            else
            {
                licenseDialog.DidNotReceive().Display(Arg.Any<LicenseDialogMessageType>());
            }
        }

        Assert.IsTrue(cache.Dictionary.ContainsValue(key));
    }
}