﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.ResultMappings.EventHandlers;

    public class ResultMappingUpdatedEventHandler : INotificationHandler<DomainEventNotification<ResultMappingUpdatedEvent>>
    {
        private readonly ILogger<ResultMappingUpdatedEventHandler> _logger;

        public ResultMappingUpdatedEventHandler(
            ILogger<ResultMappingUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<ResultMappingUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
