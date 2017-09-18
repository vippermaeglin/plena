using M4Utils;

namespace M4Core.Enums
{
    /// <summary>
    /// Master - Perfil Administrador
    /// Guest - Perfil de visualização básica do sistema
    /// </summary>
    public enum EPermission
    {
        [StringValue("NEGADO")]
        Negado = 1,
        [StringValue("RESTRINGIDO")]
        Restringido,
        [StringValue("PERMITIDO")]
        Permitido,
    }
}