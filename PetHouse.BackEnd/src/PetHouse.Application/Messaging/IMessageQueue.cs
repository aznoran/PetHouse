﻿namespace PetHouse.Application.Messaging;

public interface IMessageQueue<TMessage>
{
    Task WriteAsync(TMessage message, CancellationToken cancellationToken = default);

    Task<TMessage> ReadAsync(CancellationToken cancellationToken = default);
}