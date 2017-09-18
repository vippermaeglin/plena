namespace M4Core.Enums
{
    /// <summary>
    /// Permitted - Permitido logar offline
    /// Expired - Permissão offline expirada
    /// Blocked - Bloqueio do sistema (Causa provável: Usuário modificou a data do SO para autenticação offline ocorrer)
    /// </summary>
    public enum EStatusAuthentication
    {
        Permitted = 0,
        Expired = 1,
        Blocked = 2
    }
}