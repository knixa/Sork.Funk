# Sork.Funk

`Sork.Funk` is a C# library written as part of learning how to apply functional programming concepts in C#.<br/>
The goal of this project is to provide a practical and hands-on approach to understanding functional programming
paradigms within the context of C# and dotnet.

## Overview

The library currently includes implementations of fundamental functional programming constructs such as `Option<T>`
and `Either<TL, TR>`.<br/>
These types are used to handle optional values and represent computations that can result in two different types,
respectively.

- **Option<T>**: This type represents an optional value, a value that might exist or not.

- **Either<TL, TR>**: This type is used to represent values that can be of one of two possible types.
  It is often used to represent computations that can succeed (`Right`) or fail (`Left`).
