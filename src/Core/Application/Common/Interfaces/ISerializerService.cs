﻿namespace Microsoft.Teams.Assist.Application.Common.Interfaces;

public interface ISerializerService : ITransientService
{
    string Serialize<T>(T obj);

    string Serialize<T>(T obj, Type type);

    T Deserialize<T>(string text);
}
