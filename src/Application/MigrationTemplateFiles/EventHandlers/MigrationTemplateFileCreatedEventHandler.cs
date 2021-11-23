﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.MigrationTemplateFiles.EventHandlers;

    public class MigrationTemplateFileCreatedEventHandler : INotificationHandler<DomainEventNotification<MigrationTemplateFileCreatedEvent>>
    {
        private readonly ILogger<MigrationTemplateFileCreatedEventHandler> _logger;

        public MigrationTemplateFileCreatedEventHandler(
            ILogger<MigrationTemplateFileCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<MigrationTemplateFileCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
