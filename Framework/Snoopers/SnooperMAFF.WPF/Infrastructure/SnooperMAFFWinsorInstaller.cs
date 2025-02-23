﻿// Copyright (c) 2017 Jan Pluskal, Vit Janecek
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

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Netfox.Detective.Infrastructure;
using Netfox.SnooperMAFF.Interfaces;

namespace Netfox.SnooperMAFF.WPF.Infrastructure
{
    public class SnooperMAFFWindsorInstaller : DetectiveIvestigationWindsorInstallerBase
    {
        public SnooperMAFFWindsorInstaller() : base(GetFromAssemblyDescriptorForAssemblisDeclaringTypes(new[] { typeof(SnooperMAFF), typeof(SnooperMAFFWindsorInstaller) })) {}

        public override void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(this.FromAssemblyDescriptor.BasedOn<IWrapperConstants>().WithServiceFromInterface(typeof(IWrapperConstants)).WithServiceSelf().LifestyleTransient());
            base.Install(container, store);
        }
    }

}
