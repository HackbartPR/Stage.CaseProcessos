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
            public static readonly string EntityNotSaved = "Area could not be saved!";
            public static readonly string EntityNotFound = "Entity not found!";
        }

        public readonly struct ErrorsMessages
        {
            public static readonly string AreaNotFound = "Area was not found!";
            public static readonly string AreaNotSaved = "Area could not be saved!";
            public static readonly string UsuarioNotFound = "Usuario was not found!";
            public static readonly string ResponsibleNotFound = "Responsible was not found!";
            public static readonly string FerramentaNotFound = "Ferramenta was not found!";
            public static readonly string ProcessoNotFound = "Processo was not found!";
            public static readonly string ProcessoNotSaved = "Processo could not be saved!";
        }

        public readonly struct ValidationsMessages
        {
            public static readonly string NameField = "The 'Name' field cannot be null!";
            public static readonly string IdAreaField = "IdArea not allowed !";
            public static readonly string IdFerramentaField = "IdFerramenta not allowed !";
            public static readonly string IdField = "Id number is not valid!";
            public static readonly string IdParent = "IdParent must be null or greater than zero!";
        }
    }
}
