﻿// Copyright (c) 2017 Jan Pluskal, Matus Dobrotka
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using Castle.Core;
using Netfox.Detective.Tests;
using Netfox.SnooperMinecraft.Interfaces;
using Netfox.SnooperMinecraft.WPF.Views;
using Netfox.SnooperMinecraft.WPF.Infrastructure;
using Netfox.SnooperMinecraft.WPF.ViewModels;
using NUnit.Framework;
using MinecraftMsgView = Netfox.SnooperMinecraft.WPF.Views.MinecraftMsgView;

namespace Netfox.SnooperMinecraft.Tests
{
    [TestFixture]
    public class SnooperMinecraftWindsorInstallerTests : WindsorInvestigationInstallerTestsBase<SnooperMinecraftWindsorInstaller>
    {
        #region Overrides of WindsorInvestigationInstallerTestsBase<SnooperMinecraftWindsorInstaller>
        protected override int AutoRegisterDetectivePaneViewsRegisterRegisterTypesAllTypesAreRegisteredAssertedCount { get; } = 1;
        protected override int DetectiveInvestigationDataEntityPaneViewModelsRegisterRegisterTypesAllTypesAreRegisteredAssertedCount { get; } = 2;
        protected override int InstallRegisterTypesAllTypesAreRegisteredAssertedCount { get; } = 11;
        protected override int RegisterOtherTypesRegisterTypesAllTypesAreRegisteredAssertedCount { get; } = 1;
        protected override int SnooperRegisterRegisterTypesAllTypesAreRegisteredAssertedCount { get; } = 2;
        protected override int RegisterServicesRegisterServicesDetectiveScopeServicesAreRegisteredAssertedCount { get; } = 1;
        protected override int AnalyzersRegistrationInvestigationAnalyzerRegisterInvestigationAnalyzerRegisterAreRegisteredAssertedCount { get; } = 1;

        protected override void AssertIEntityViewModels() { Assert.IsTrue(this.ContainsComponent(typeof(MinecraftMessageViewModel), LifestyleType.Custom)); }

        protected override void AssertServices() { }

        protected override void AssertViews() { Assert.IsTrue(this.ContainsComponent(typeof(IMinecraftMsgView), typeof(MinecraftMsgView), LifestyleType.Transient)); }

        protected override int DetectiveInvestigationPaneViewModelsRegisterRegisterTypesAllTypesAreRegisteredCount { get; } = 1;
        #endregion
    }
}
