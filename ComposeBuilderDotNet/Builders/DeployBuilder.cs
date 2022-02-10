﻿using ComposeBuilderDotNet.Builders.Base;
using ComposeBuilderDotNet.Enums;
using ComposeBuilderDotNet.Model;
using System;
using System.Collections.Generic;

namespace ComposeBuilderDotNet.Builders
{
    public class DeployBuilder : BaseBuilder<DeployBuilder, Deploy>
    {
        internal DeployBuilder()
        {
        }

        public DeployBuilder WithLabels(IDictionary<string, string> labels)
        {
            WorkingObject.Labels ??= new Dictionary<string, string>();

            return AddToDictionary(WorkingObject.Labels, labels);
        }

        public DeployBuilder WithLabels(Action<IDictionary<string, string>> environmentExpression)
        {
            WorkingObject.Labels ??= new Dictionary<string, string>();

            environmentExpression(WorkingObject.Labels);

            return this;
        }

        public DeployBuilder WithMode(EReplicationMode mode)
        {
            WorkingObject.Mode = mode;
            return this;
        }

        public DeployBuilder WithReplicas(int replicas)
        {
            WorkingObject.Replicas = replicas;
            return this;
        }

        public DeployBuilder WithUpdateConfig(Action<MapBuilder> updateConfig)
        {
            var mb = new MapBuilder().WithName("update_config");
            updateConfig(mb);
            WorkingObject.UpdateConfig = mb.Build();
            return this;
        }

        public DeployBuilder WithRestartPolicy(Action<MapBuilder> restartPolicy)
        {
            var mb = new MapBuilder().WithName("restart_policy");
            restartPolicy(mb);
            WorkingObject.RestartPolicy = mb.Build();
            return this;
        }

        public DeployBuilder WithPlacement(Action<MapBuilder> placement)
        {
            var mb = new MapBuilder().WithName("placement");
            placement(mb);
            WorkingObject.Placement = mb.Build();
            return this;
        }

        public DeployBuilder WithResources(Action<MapBuilder> resources)
        {
            var mb = new MapBuilder().WithName("resources");
            resources(mb);
            WorkingObject.Resources = mb.Build();
            return this;
        }
    }
}
