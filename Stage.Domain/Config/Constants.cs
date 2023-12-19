namespace Stage.Domain.Config
{
    public static class Constants
    {
        public readonly struct GenericProperties
        {
            public static readonly string Active = "Active";
            public static readonly string CreatedAt = "CreatedAt";
            public static readonly string UpdatedAt = "UpdatedAt";
        }

        public readonly struct ErrorsKeys
        {
            public static readonly string AreaNotFound = "Area not found!";
            public static readonly string AreaNotSaved = "Area could not be saved!";
            public static readonly string UsuarioNotFound = "Usuario not found!";
        }

        public readonly struct ErrorsMessages
        {
            public static readonly string AreaNotFound = "Area was not found!";
            public static readonly string AreaNotSaved = "Area could not be saved!";
            public static readonly string UsuarioNotFound = "Usuario was not found!";
        }

        public readonly struct ValidationsMessages
        {
            public static readonly string NameField = "The 'Name' field cannot be null!";
            public static readonly string IdAreaField = "Not allowed IdArea!";
            public static readonly string IdField = "Id number is not valid!";
        }
    }
}
