﻿using SharpAnyType;

namespace QuarkConstants;

public static class QuarkConstants
{
    public static Any GetNil() => Any.Nil;

    public static Any IsNil(Any any) => (any == GetNil()).ObjectToAny();
}