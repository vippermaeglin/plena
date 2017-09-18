using System.ComponentModel;
using M4.M4v2.Chart.IndicatorElements;
using System;
using System.Security;
using System.Collections.Generic;

namespace M4.M4v2.Settings
{
    /// <summary>
    /// Para ordenação dos grupos é necessário ordenar as categorias. 
    /// Ex.: Código (CA = Categoria A) + Nome do Grupo
    /// </summary>
    public class UserInfo
    {
        #region Basic
        
        [Category("CA-Basic")]
        public string FirstName { get; set; }
        [Category("CA-Basic")]
        public string LastName { get; set; }
        [Category("CA-Basic")]
        public string CPF { get; set; }
        [Category("CA-Basic")]
        public string Email { get; set; }
        [Category("CA-Basic")]
        public string UserName { get; set; }
        [Category("CA-Basic")]
        public string OldPassword { get; set; }
        [Category("CA-Basic")]
        public string NewPassword { get; set; }
        [Category("CA-Basic")]
        public string NewPassword2 { get; set; }

        
        #endregion

        #region Optional

        [Category("CB-Optional")]
        public DateTime Birthday { get; set; }
        [Category("CB-Optional")]
        public string CEP { get; set; }
        [Category("CB-Optional")]
        public EnumStates.Enum State { get; set; }
        [Category("CB-Optional")]
        public string City { get; set; }
        [Category("CB-Optional")]
        public string District { get; set; }
        [Category("CB-Optional")]
        public string Street { get; set; }
        [Category("CB-Optional")]
        public string Number { get; set; }
        [Category("CB-Optional")]
        public string Complement { get; set; }

        #endregion

    }

    public class StringPass{
        public string Value;
        public string Show = "*****";
        public StringPass(string value) { Value = value; }
    }

    public class EnumStates
    {
        public static string[] States = {
                                            "",
                                    "Acre (AC)",
                                    "Alagoas (AL)",
                                    "Amapá (AP)",
                                    "Amazonas (AM)",
                                    "Bahia (BA)",
                                    "Ceará (CE)",
                                    "Distrito Federal (DF)",
                                    "Espírito Santo (ES)",
                                    "Goiás (GO)",
                                    "Maranhão (MA)",
                                    "Mato Grosso (MT)",
                                    "Mato Grosso do Sul (MS)",
                                    "Minas Gerais (MG)",
                                    "Pará (PA) ",
                                    "Paraíba (PB)",
                                    "Paraná (PR)",
                                    "Pernambuco (PE)",
                                    "Piauí (PI)",
                                    "Rio de Janeiro (RJ)",
                                    "Rio Grande do Norte (RN)",
                                    "Rio Grande do Sul (RS)",
                                    "Rondônia (RO)",
                                    "Roraima (RR)",
                                    "Santa Catarina (SC)",
                                    "São Paulo (SP)",
                                    "Sergipe (SE)",
                                    "Tocantins (TO)"
                                 };
        public enum Enum
        {
            _,
            AC,
            AL,
            AP,
            AM,
            BA,
            CE,
            DF,
            ES,
            GO,
            MA,
            MT,
            MS,
            MG,
            PA,
            PB,
            PR,
            PE,
            PI,
            RJ,
            RN,
            RS,
            RO,
            RR,
            SC,
            SP,
            SE,
            TO
        }
    }
}
