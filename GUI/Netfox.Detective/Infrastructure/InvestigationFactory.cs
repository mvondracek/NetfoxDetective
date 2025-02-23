﻿// Copyright (c) 2017 Jan Pluskal, Martin Mares, Martin Kmet, Hana Slamova
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

using System.Threading.Tasks;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Netfox.Core.Infrastructure;
using Netfox.Core.Interfaces;
using Netfox.Core.Models;
using Netfox.Detective.Interfaces;
using Netfox.Detective.Interfaces.Models.Base;
using Netfox.Detective.Messages;
using Netfox.Detective.Models.Base;

namespace Netfox.Detective.Infrastructure {
    internal class InvestigationFactory: IInvestigationFactory {
        public IWindsorContainer Container { get; }
        private IWindsorContainer chidWc;
        private IDetectiveMessenger _messenger;
        private ISerializationPersistor<Investigation> _investigationSerializationPersistor;
        public InvestigationFactory(IWindsorContainer container, IDetectiveMessenger messenger, ISerializationPersistor<Investigation> investigationSerializationPersistor)
        {
            this.Container = container;
            this._messenger = messenger;
            this._investigationSerializationPersistor = investigationSerializationPersistor;

        }
        #region Implementation of IInvestigationFactory
        public IInvestigation CreateInternal(IInvestigationInfo investigationInfo)
        {
            var chidWc = new WindsorContainer($"Investigation-{investigationInfo.InvestigationName}-{investigationInfo.Guid}", new DefaultKernel(), new DefaultComponentInstaller());
            this.Container.AddChildContainer(chidWc);
            chidWc.Register(Component.For<IInvestigationInfo,InvestigationInfo>().Instance(investigationInfo));
            chidWc.Register(Component.For<IWindsorContainer, WindsorContainer>().Instance(chidWc));
            chidWc.Install(FromAssembly.InDirectory(new AssemblyFilter("."), new InstallerFactoryFilter(typeof(IDetectiveIvestigationWindsorInstaller))));
            var investigationInfoToReturn =  chidWc.Resolve<IInvestigationFactoryInternal>().CreateInternal(investigationInfo);

            
            return investigationInfoToReturn;
        }

        public async Task<IInvestigation> Create(IInvestigationInfo investigationInfo)
        {
            chidWc = new WindsorContainer($"Investigation-{investigationInfo.InvestigationName}-{investigationInfo.Guid}", new DefaultKernel(), new DefaultComponentInstaller());
            this.Container.AddChildContainer(chidWc);
            chidWc.Register(Component.For<IInvestigationInfo, InvestigationInfo>().Instance(investigationInfo));
            chidWc.Register(Component.For<IWindsorContainer, WindsorContainer>().Instance(chidWc));
            chidWc.Register(Component.For<IDetectiveMessenger>().Instance(this._messenger));
            chidWc.Register(Component.For<ISerializationPersistor<Investigation>>().Instance(this._investigationSerializationPersistor));
            
            chidWc.Install(FromAssembly.InDirectory(new AssemblyFilter("."), new InstallerFactoryFilter(typeof(IDetectiveIvestigationWindsorInstaller))));
            var investigation = chidWc.Resolve<IInvestigationFactoryInternal>().CreateInternal(investigationInfo);

            investigationInfo.CreateFileStructure();
            await investigation.Initialize();

            return investigation;
        }
        #endregion
    }
}