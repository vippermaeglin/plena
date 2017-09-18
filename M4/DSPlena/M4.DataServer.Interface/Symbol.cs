using System.Collections.Generic;

namespace M4.DataServer.Interface
{
    public enum FonteDados
    {
        NULL,
        BARCHART,
        PLENA
    }

    public enum Motivo
    {
        NULL,
        SPLIT,
        AGRUPAMENTO,
        DIVIDENDO
    }

    public enum Setor
    {
        NULL,
        CONSUMO_NAO_CICLICO,
        MATERIAIS_BASICOS,
        FINENCEIROS_OUTROS,
        BENS_INDUSTRIAIS,
        PETROLEO_GAS_BIOCOMBUSTIVEIS,
        CONSTRUCAO_TRANSPORTE,
        UTILIDADE_PUBLICA,
        CONCUMO_CICLICO
    }

    public enum SubSetor
    {
        NULL,
        BEBIDAS,
        COMERCIO,
        MIDIA,
        SAUDE,
        MADEIRA_PAPEL,
        INTERMEDIARIOS_FINANCEIROS,
        EXPLORACAO_IMOVEIS,
        MATERIAL_TRANSPORTE,
        PETROLEO_GAS_BIOCOMBUSTIVEIS,
        CONSTRUCAO_ENGENHARIA,
        DIVERSOS,
        ENERGIA_ELETRICA,
        VIAGENS_LAZER
    }

    public enum Segmento
    {
        NULL,
        CERVEJAS_REFRIGERANTES,
        JORNAIS_LIVROS_REVISTAS,
        SERV_MED_HOSPIT_ANALISES_DIAGNOSTICOS,
        PAPEL_CELULOSE,
        BANCOS,
        EXPLORACAO_IMVOVEIS,
        MATERIAL_RODOVIARIO,
        EXPLORACAO_REFINO,
        CONSTRUCAO_CIVIL,
        SERVICOS_EDUCACIONAIS,
        ENERGIA_ELETRICA,
        PRODUCAO_EVENTOS_SHOWS
    }

    public enum Tipo
    {
        NULL,
        PN,
        ON,
        DP,
        DO
    }

    public enum TipoRegistro
    {
        AJUSTADO,
        NAO_AJUSTADO
    }

    public enum Priority
    {
        Low,
        High
    }

    public enum GroupType
    {
        Portfolio,
        Index
    }

    /// <summary>
    /// Structure with symbols information
    /// </summary>
  //  [Persistent(List = true)]
    public class Symbol
    {
        #region Propriedades

     //   [Indexable(Unique = true)] // create unique (tree) eXtremeDB index by "Code" field
        public string Code;

        public string Name;

        public string Sector;

        public string SubSector;

        public string Segment;

        public string Source;

        public string Type;

        public string Activity;

        public string Site;

        public int Status;

        public int Priority;

        #endregion
        
        public Symbol()
        {

        }

        public Symbol(string name, string code, string sector, string subsector, string segment, string source, string type, string activity, string site, int status, int priority)
        {
            Name = name;
            Code = code;
            Sector = sector;
            SubSector = subsector;
            Segment = segment;
            Source = source;
            Type = type;
            Activity = activity;
            Site = site;
            Status = status;
            Priority = priority;
        }
        
        public bool Equals(Symbol other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Code.Equals(Code) && other.Name == Name && other.Sector == Sector && other.SubSector == SubSector &&
                   other.Source == Source && other.Type == Type && other.Activity == Activity && other.Site == Site && other.Status == Status && other.Priority == Priority;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Symbol)) return false;
            return Equals((Symbol)obj);
        }

    }

    /// <summary>
    /// Structure used to group symbols by index or portfolio
    /// </summary>
  //  [Persistent(List = true)]
    public class SymbolGroup
    {
    //    [Indexable(Unique = true)] // create unique (tree) eXtremeDB index by "Code" field
        public string Name;
        public int Index;
        public int Type; // 0 = User Portfolio , 1 = Market Index
        public string Symbols;
        public SymbolGroup()
        {
            
        }
        public SymbolGroup(string name, int index, int type, List<string>symbols)
        {
            Name = name;
            Index = index;
            Type = type;
            Symbols = "";
            if (symbols.Count > 0)
            {
                Symbols = symbols[0];
                for (int i = 1; i < symbols.Count; i++)
                {
                    Symbols += "," + symbols[i];
                }
            }
        }
    }


}
