﻿// Copyright (c) 2017 Jan Pluskal, Martin Mares, Martin Kmet
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

using Castle.Windsor;
using GalaSoft.MvvmLight.Threading;
using Netfox.Core.Interfaces.Views;
using Netfox.Detective.ViewModelsDataEntity;
using Netfox.Detective.ViewModelsDataEntity.Exports;

namespace Netfox.Detective.ViewModels.Exports
{
    public class ExportOverviewVm : DetectiveIvestigationDataEntityPaneViewModelBase
    {
        public ExportOverviewVm(WindsorContainer investigationWindsorContainer, ExportGroupVm model) : base(investigationWindsorContainer, model)
        {
            this.ExportGroupVm = model;
            DispatcherHelper.CheckBeginInvokeOnUI(() => this.View = this.ApplicationOrInvestigationWindsorContainer.Resolve<IExportOverviewView>());
        }

        #region Overrides of DetectivePaneViewModelBase
        public override string HeaderText => "Export overview";
        #endregion

        public ExportGroupVm ExportGroupVm { get; set; }
    }
}