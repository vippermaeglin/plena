
namespace M4Utils.Language
{
    public class PortugueseBrazil : LanguageDefault
    {
        public PortugueseBrazil()
        {
            LoadPlena();
            LoadDictionaryAssets();
            LoadTabAssets();
            LoadMenuAssets();
            LoadChartCtl();
            LoadMenuBar();
            LoadMessage();
            LoadTitleMessage();
            LoadSelectChart();
            LoadSelectIndicator();
            LoadSelectStudy();
            LoadBrowser();
            LoadSelectTools();
            LoadSettings();
            LoadPortfolio();
            LoadMenuPlena();
            LoadLogin();
            LoadPermission();
            LoadTemplate();
            LoadWorkspace();
            LoadOutput();
        }


        public override sealed void LoadPlena()
        {
            DictionaryPlena.Add("webBrowser", "Web");
            DictionaryPlena.Add("tradeDiary", "Diário de Trades");
            DictionaryPlena.Add("statusManager", "Monitor");
            DictionaryPlena.Add("about", "\nCopyright © 2012-2014 Seamus Consultoria S/C Ltda. Todos os direitos reservados.\n\n\n\n\nAviso: Este software está protegido pela lei de direitos autorais e tratados internacionais.Reprodução ou distribuição não autorizada deste programa, ou qualquer parte dele, pode resultar em severas penalidades civis e criminais.");
        }

        public override sealed void LoadMenuPlena()
        {
            DictionaryMenuPlena.Add("clearSorting", "Desfazer Ordenação");
            DictionaryMenuPlena.Add("mnuMenuFile", "Arquivo");
            DictionaryMenuPlena.Add("mnuLoadChartDisk", "Carregar Gráfico");
            DictionaryMenuPlena.Add("mnuSaveChart", "Salvar Gráfico");
            DictionaryMenuPlena.Add("mnuSaveChartImage", "Salvar Imagem");
            DictionaryMenuPlena.Add("mnuPrintChart", "Imprimir");
            DictionaryMenuPlena.Add("mnuExit", "Sair");
            DictionaryMenuPlena.Add("mnuMenuHelp", "Ajuda");
            DictionaryMenuPlena.Add("mnuHelpContent", "Conteúdo");
            DictionaryMenuPlena.Add("mnuAboutPLENA", "Sobre");
            DictionaryMenuPlena.Add("mnuMenuWorkspace", "Área de Trabalho");
            DictionaryMenuPlena.Add("mnuManager", "Gerenciador");
            DictionaryMenuPlena.Add("mnuMenuChart", "Gráfico && Operações");
            DictionaryMenuPlena.Add("mnuPopoutActiveChart", "Destacar Janela");
            DictionaryMenuPlena.Add("mnuTradesTweets", "Publicar Tweets");
            DictionaryMenuPlena.Add("mnuMenuView", "Exibir");
            DictionaryMenuPlena.Add("mnuToolbar", "Componentes");
            DictionaryMenuPlena.Add("mnuCalculator", "Calculadora");
            DictionaryMenuPlena.Add("mnuChartTools", "Estudos e Acessórios");
            //DictionaryMenuPlena.Add("mnuApplicationStyle", "Estilo && Cores");
            DictionaryMenuPlena.Add("mnuChartColors", "Esquema de Cores");
            DictionaryMenuPlena.Add("mnuApplicationStyle", "Estilo");
            DictionaryMenuPlena.Add("mnuUseSemiLogScale", "Escala Logaritmica");
            DictionaryMenuPlena.Add("mnuShowXGrid", "Gride p/ Eixo X");
            DictionaryMenuPlena.Add("mnuShowYGrid", "Gride p/ Eixo Y");
            DictionaryMenuPlena.Add("mnuShowPanelSeparators", "Separador de Painéis");
            DictionaryMenuPlena.Add("mnuCrosshair", "Mouse em Cruz");
            DictionaryMenuPlena.Add("mnuDarvasBoxes", "Darvas Boxes");
            DictionaryMenuPlena.Add("mnuViewStartPag", "Página Inicial");
            DictionaryMenuPlena.Add("mnuSettings", "Configurações");
            DictionaryMenuPlena.Add("mnuCloseWindow", "Fechar Guia");
            DictionaryMenuPlena.Add("mnuCloseAllButThis", "Fechar outras Guias");
            DictionaryMenuPlena.Add("mnuCloseAll", "Fechar Guias");
            DictionaryMenuPlena.Add("mnuFloat", "Destacar");
            DictionaryMenuPlena.Add("mnuFloating", "Destacada");
            DictionaryMenuPlena.Add("mnuDocked", "Docar");
            DictionaryMenuPlena.Add("mnuSortWindow", "Ordenar Janelas");
            DictionaryMenuPlena.Add("mnuAutoHide", "Esconder Automaticamente");
            DictionaryMenuPlena.Add("mnuClearSorting", "Remover ordenação");
            DictionaryMenuPlena.Add("mnuAddRemoveButtonsMenuItem", "Adicionar ou Remover Botões");
            DictionaryMenuPlena.Add("mnuPanelPosition", "Posicionar Panel");
            DictionaryMenuPlena.Add("mnuPanelDown", "Abaixo");
            DictionaryMenuPlena.Add("mnuPanelUp", "Acima");

        }

        /// <summary>
        /// Carrega os valores do dicionário
        /// </summary>
        public override sealed void LoadDictionaryAssets()
        {
            #region GridAssets

            DictionaryGridAssets.Add("columnActive", "Ativo");
            DictionaryGridAssets.Add("columnAmount", "Quantidade");
            DictionaryGridAssets.Add("columnBusiness", "Negócios");
            DictionaryGridAssets.Add("columnClosing", "Fechamento");
            DictionaryGridAssets.Add("columnHour", "Hora");
            DictionaryGridAssets.Add("columnLast", "Último");
            DictionaryGridAssets.Add("columnMaximum", "Máximo");
            DictionaryGridAssets.Add("columnMinimum", "Mínimo");
            DictionaryGridAssets.Add("columnOpening", "Abertura");
            DictionaryGridAssets.Add("columnVariation", "Variação");
            DictionaryGridAssets.Add("columnVolume", "Volume");
            DictionaryGridAssets.Add("messageDeleteRecord", "Deseja excluir esse registro?");
            DictionaryGridAssets.Add("titleDeleteRecord", "Excluir Registro");

            #endregion
        }

        /// <summary>
        /// Carrega os valores do dicionário de tabs dos ativos
        /// </summary>
        public override sealed void LoadTabAssets()
        {
            #region GridAssets

            DictionaryTabAssets.Add("tabAll", "Todos");
            DictionaryTabAssets.Add("newTab", "Nova Aba");

            #endregion
        }

        /// <summary>
        /// Carrega os valores do dicionário do menu dos ativos
        /// </summary>
        public override sealed void LoadMenuAssets()
        {
            #region MenuAssets

            DictionaryMenuAssets.Add("btnNewWallet", "Nova Carteira");
            DictionaryMenuAssets.Add("btnDeleteWallet", "Deletar Carteira");

            #endregion
        }

        /// <summary>
        /// Carrega os valores do dicionário da tela do gráfico
        /// </summary>
        public override sealed void LoadChartCtl()
        {
            #region Chart

            DictionaryChartCtl.Add("twSumary", "Sumário");
            DictionaryChartCtl.Add("twAdvisors", "Conselheiros");

            #endregion
        }

        /// <summary>
        /// Carrega os valores do dicionário do menu principal
        /// </summary>
        public override sealed void LoadMenuBar()
        {
            #region MenuBar

            DictionaryMenuBar.Add("cmdPositionFlaps", "Posição Abas");

            DictionaryMenuBar.Add("cbxApplicationStyle", "Estilo da Aplicação");
            DictionaryMenuBar.Add("optOffice2007Blue", "Office 2007 Blue");
            DictionaryMenuBar.Add("optOffice2007Silver", "Office 2007 Silver");
            DictionaryMenuBar.Add("optWindowsVista", "Windows Vista");
            DictionaryMenuBar.Add("optWindowsDefault", "Windows Default");

            DictionaryMenuBar.Add("cmdUseSemiLogScale", "Uso Semi-Log de Escala");
            DictionaryMenuBar.Add("cmdShowXGrid", "Mostrar X Grade");
            DictionaryMenuBar.Add("cmdShowYGrid", "Mostrar Y Grade");
            DictionaryMenuBar.Add("cmdShowPanelSeparators", "Mostrar Separadores de Painéis");
            DictionaryMenuBar.Add("cmdCrosshair", "Mouse em Cruz");
            DictionaryMenuBar.Add("cmdThreeDStyle", "Estilo Três D");
            DictionaryMenuBar.Add("cmdDarvasBoxes", "Darvas Boxes");
            DictionaryMenuBar.Add("cmdViewStarPage", "Ver StarPage");
            DictionaryMenuBar.Add("cmdViewForexScreen", "Ver Tela Forex");
            DictionaryMenuBar.Add("cmdTextObject", "Texto");
            DictionaryMenuBar.Add("cmdBuySymbol", "Sinal de Compra");
            DictionaryMenuBar.Add("cmdSellSymbol", "Sinal de Venda");
            DictionaryMenuBar.Add("cmdExitSymbol", "Sinal de Saída");
            DictionaryMenuBar.Add("cmdLineColor", "Cor da Linha");
            DictionaryMenuBar.Add("cmdTrendLine", "Linha de Tendência");
            DictionaryMenuBar.Add("cmdEllipse", "Elipse");
            DictionaryMenuBar.Add("cmdSpeedLines", "Linhas de Velocidade");
            DictionaryMenuBar.Add("cmdGannFan", "Gann Fan");
            DictionaryMenuBar.Add("cmdChart", "Novo Gráfico");
            DictionaryMenuBar.Add("cmdFibonacciArcs", "Arcos de Fibonacci");
            DictionaryMenuBar.Add("cmdFibonacciRetracements", "Retração de Fibonacci");
            DictionaryMenuBar.Add("cmdFibonacciFan", "Fibonacci Fan");
            DictionaryMenuBar.Add("cmdFibonacciTimeZones", "Linhas de Tempo de Fibonacci");
            DictionaryMenuBar.Add("cmdTironeLevels", "Níveis de Tirone");
            DictionaryMenuBar.Add("cmdQuadrantLines", "Quadrante de Linhas");
            DictionaryMenuBar.Add("cmdRaffRegression", "Raff Regression");
            DictionaryMenuBar.Add("cmdErrorChannels", "Canais de Erros");
            DictionaryMenuBar.Add("cmdRectangle", "Retângulo");
            DictionaryMenuBar.Add("cmdArrow", "Seta");
            DictionaryMenuBar.Add("cmdFreeHandDrawing", "Desenho Livre");
            DictionaryMenuBar.Add("cmdZoomArea", "Zoom Área");
            DictionaryMenuBar.Add("cmdZoomZero", "Visão Geral");
            DictionaryMenuBar.Add("cmdZoomIn", "Zoom In");
            DictionaryMenuBar.Add("cmdZoomOut", "Zoom Out");
            DictionaryMenuBar.Add("cmdCandleChart", "Candle Chart");
            DictionaryMenuBar.Add("cmdStockLine", "Linha");
            DictionaryMenuBar.Add("cmdBarChart", "Barra");
            DictionaryMenuBar.Add("cmdHeikinAshi", "Heikin Ashi");
            DictionaryMenuBar.Add("cmdHeikinAshiSmooth", "Heikin Ashi Suavizado");
            DictionaryMenuBar.Add("cmdDeleteStock", "Apagar Estudos");
            DictionaryMenuBar.Add("cmdTechnicalAnalysis", "Indicadores");
            DictionaryMenuBar.Add("cmdPriceSeries", "Opções de Preço");

            DictionaryMenuBar.Add("mnuToolbar", "Opções");
            DictionaryMenuBar.Add("mnuPriceIndicatorsToolbar", "Preço && Indicadores");
            DictionaryMenuBar.Add("mnuZoomTemplatesToolbar", "Zoom && Templates");
            DictionaryMenuBar.Add("mnuCalculatorToolbar", "Calculadora");
            DictionaryMenuBar.Add("mnuViewToolbar", "Visão");
            DictionaryMenuBar.Add("mnuChartToolsToolbar", "Ferramentas de Gráfico");
            DictionaryMenuBar.Add("mnuFileToolbar", "Arquivo");

            DictionaryMenuBar.Add("cmdSelect", "Mouse em Ponteiro");
            DictionaryMenuBar.Add("cmdDeltaCursor", "Régua");
            DictionaryMenuBar.Add("cmdMagnetic", "Magnética");
            DictionaryMenuBar.Add("cmdRay", "Linha Extendida");
            DictionaryMenuBar.Add("cmdChannel", "Canal");
            DictionaryMenuBar.Add("cmdHorizontalLine", "Linha Horizontal");
            DictionaryMenuBar.Add("cmdVerticalLine", "Linha Vertical");
            DictionaryMenuBar.Add("cmdElipse", "Elipse");
            DictionaryMenuBar.Add("cmdPolyline", "Polilinha");
            DictionaryMenuBar.Add("cmdFibonacciProjections", "Projeção de Fibonacci");

            DictionaryMenuBar.Add("cmdPeriodicityDaily", "Diário");
            DictionaryMenuBar.Add("cmdPeriodicityMonth", "Mensal");
            DictionaryMenuBar.Add("cmdPeriodicityWeekly", "Semanal");
            DictionaryMenuBar.Add("cmdPeriodicityYearly", "Anual");
            DictionaryMenuBar.Add("cmdPeriodicityCustom", "Customizar");
            DictionaryMenuBar.Add("cmdPeriodicityAsk", "(Perguntar)");

            DictionaryMenuBar.Add("titleTabDaily", "Dia");
            DictionaryMenuBar.Add("titleTabWeekly", "Sem");
            DictionaryMenuBar.Add("titleTabMonthly", "Mês");
            DictionaryMenuBar.Add("titleTabYearly", "Ano");

            DictionaryMenuBar.Add("titleShortTabDaily", "Dia");
            DictionaryMenuBar.Add("titleShortTabWeekly", "Sem");
            DictionaryMenuBar.Add("titleShortTabMonthly", "Mês");
            DictionaryMenuBar.Add("titleShortTabYearly", "Ano");

            DictionaryMenuBar.Add("ndtChartTools", "Ferramentas de Gráfico");

            #endregion
        }

        public override sealed void LoadMessage()
        {
            DictionaryMessage.Add("msgWarningTitle", "Aviso");
            DictionaryMessage.Add("msgSelectStock", "Selecione um ativo para continuar");
            DictionaryMessage.Add("msgSelectPeriodicity", "Selecione uma periocidade para continuar");
            DictionaryMessage.Add("msgErrLoadStockLocalData", "Ativo não encontrado em nossa base de dados.");
            DictionaryMessage.Add("msgGenerateDataWeekly", "Houve um erro na conversão de dados.");
            DictionaryMessage.Add("msgCandlesMinimum", "Periodicidade solicitada não gerou o número mínimo de candles para exibição.");
            DictionaryMessage.Add("msgIntervalMaximum", "O intervalo para periodicidade intra-diário não pode ser inferior a 1 ou superior a 420 minutos.");
            DictionaryMessage.Add("msgHistoryMaximum", "O intervalo para história intra-diário não pode ser inferior a 2 ou superior a 15000.");
            DictionaryMessage.Add("msgSelectIndicator", "Selecione um indicador para continuar");
            DictionaryMessage.Add("msgPeriodsMinimum", "O período informado tem que ser maior que 0.");
            DictionaryMessage.Add("msgCycleMinimum", "O ciclo informado tem que ser maior que 0.");
            DictionaryMessage.Add("msgPeriodsIsEmpty", "O período informado não pode ser inferior a 0 ou superior a 999.");
            DictionaryMessage.Add("msgThresholdIsEmpty", "O limite deve ser um valor entre 0 e 100.");
            DictionaryMessage.Add("msgCycleIsEmpty", "O ciclo informado não pode ser vazio.");
            DictionaryMessage.Add("msgRemoveIndicatorMock", "Deseja realmente remover o indicator selecionado?");
            DictionaryMessage.Add("msgIndicatorChanged", "Indicador alterado com sucesso!");
            DictionaryMessage.Add("msgIndicatorInserted", "Indicador inserido com sucesso!");
            DictionaryMessage.Add("msgExistingBookmark", "Indicador existente!");
            DictionaryMessage.Add("msgNotRootRemove", "Não é possível remover registros que contém itens.");
            DictionaryMessage.Add("msgTemplateAdded", "Modelo adicionado com sucesso!");
            DictionaryMessage.Add("msgTemplateDeleted", "Modelo removido com sucesso!");
            DictionaryMessage.Add("msgTemplateExists", "Existe informações com essa descrição.");
            DictionaryMessage.Add("msgWorkspaceExists", "Existe informações com essa descrição.");
            DictionaryMessage.Add("msgTemplateChanged", "Modelo alterado com sucesso!");
            DictionaryMessage.Add("msgTemplateEmpty", "Insira um valor no campo modelo!");
            DictionaryMessage.Add("msgWorkspaceEmpty", "Insira um valor no campo workspace!");
            DictionaryMessage.Add("msgTemplateMainNotRemoved", "Modelo principal não pode ser removido.");
            DictionaryMessage.Add("msgWorkspaceMainNotRemoved", "Workspace principal não pode ser removido.");
            DictionaryMessage.Add("msgRemoveWorkspace", "Deseja realmente remover a área de trabalho selecionada?");
            DictionaryMessage.Add("msgRemoveNodeTemplate", "Deseja realmente remover o Template selecionado?");
            DictionaryMessage.Add("msgRemoveAllDrawings", "Deseja remover todos os estudos/desenhos?");
            DictionaryMessage.Add("msgInvalidPeriodicity", "Não há dados para esta periodicidade em ");
        }

        public override sealed void LoadTitleMessage()
        {
            DictionaryTitleMessage.Add("titleAnswerError", "Erro de resposta");
        }

        public override sealed void LoadSelectChart()
        {
            DictionarySelectChart.Add("lblDescriptionSymbol", "Ativo");
            DictionarySelectChart.Add("lblDescriptionPeriodicity", "Periodicidade");
            DictionarySelectChart.Add("lblAdvanced", "Avançado");
            DictionarySelectChart.Add("lblAdvancedDescriptionPeriodicity", "Periodicidade");
            DictionarySelectChart.Add("lblAdvancedDescriptionHistory", "Num Barras");
            DictionarySelectChart.Add("lblAdvancedDescriptionInterval", "Intervalo");
            DictionarySelectChart.Add("btnOk", "Ok");
            DictionarySelectChart.Add("btnCancel", "Cancelar");
            DictionarySelectChart.Add("btnDismiss", "Fechar");
            DictionarySelectChart.Add("btnWeek", "S");
            DictionarySelectChart.Add("btnYear", "A");
            DictionarySelectChart.Add("btnAdvancedWeek", "S");
            DictionarySelectChart.Add("btnAdvancedYear", "A");
            DictionarySelectChart.Add("FrmSelectChartTitle", "Novo Gráfico");
            DictionarySelectChart.Add("msgTemplateExists", "Template já existe!");
            DictionarySelectChart.Add("msgWorkspaceExists", "Workspace já existe!");
            DictionarySelectChart.Add("mnuEdit", "Editar");
            DictionarySelectChart.Add("mnuDelete", "Remover");
        }

        public override sealed void LoadSelectIndicator()
        {
			DictionarySelectIndicator.Add("ErrorPanel", "Não há espaço no gráfico para mais um indicador, remova um dos painéis!");
            DictionarySelectIndicator.Add("btnNew", "Novo Indicador");
            DictionarySelectIndicator.Add("btnRemove", "Remover Indicador");
            DictionarySelectIndicator.Add("btnApply", "Aplicar Alterações");
            //Indicators Parameters
            DictionarySelectIndicator.Add("FrmSelectIndicatorTitle", "Indicadores");
            DictionarySelectIndicator.Add("lblShortCycle", "Ciclo Curto");
            DictionarySelectIndicator.Add("lblLongCycle", "Ciclo Longo");
            DictionarySelectIndicator.Add("lblPeriods", "Períodos");
            DictionarySelectIndicator.Add("lblColor", "Cor");
            DictionarySelectIndicator.Add("lblLineStyle", "Estilo da Linha");
            DictionarySelectIndicator.Add("lblLineThickness", "Espessura da Linha");
            DictionarySelectIndicator.Add("lblWindow", "Janela");
            DictionarySelectIndicator.Add("lblSourceAverage", "Fonte");
            DictionarySelectIndicator.Add("lblIndicators", "Indicadores");
            DictionarySelectIndicator.Add("lblScaleAverage", "Escala");
            DictionarySelectIndicator.Add("lblLimitMoveValue", "Valor de Mudança Limite");
            DictionarySelectIndicator.Add("lblMinimumTickValue", "Crédito Valor Mínimo");
            DictionarySelectIndicator.Add("lblSourceParameters", "Fonte");
            DictionarySelectIndicator.Add("lblVolumeSource", "Volume");
            DictionarySelectIndicator.Add("lblLongTermParameters", "Longo Prazo");
            DictionarySelectIndicator.Add("lblShortTermParameters", "Curto Prazo");
            DictionarySelectIndicator.Add("lblPointsPercentsParameters", "Pontos ou porcentagens");
            DictionarySelectIndicator.Add("lblCycle1", "Ciclo 1");
            DictionarySelectIndicator.Add("lblCycle2", "Ciclo 2");
            DictionarySelectIndicator.Add("lblCycle3", "Ciclo 3");
            DictionarySelectIndicator.Add("lblStandardDev", "Desvio");
            DictionarySelectIndicator.Add("lblDeviation", "Deslocamento");
            DictionarySelectIndicator.Add("lblPercentage", "Percentual");
            DictionarySelectIndicator.Add("lblType", "MA Tipo");
            DictionarySelectIndicator.Add("lblSourceComparativeParameters", "Fonte 2");
            DictionarySelectIndicator.Add("lblLevels", "Níveis");
            DictionarySelectIndicator.Add("lblMinAf", "Min AF");
            DictionarySelectIndicator.Add("lblMaxAf", "Max AF");
            DictionarySelectIndicator.Add("lblRatOfChg", "Taxa de Variação");
            DictionarySelectIndicator.Add("lblShiftParameters", "Deslocar");
            DictionarySelectIndicator.Add("lblBarHistoryParameters", "Barra de História");
            DictionarySelectIndicator.Add("lblKPeriodsParameters", "%K Períodos");
            DictionarySelectIndicator.Add("lblKSlowingParameters", "%K Retardando");
            DictionarySelectIndicator.Add("lblDPeriodsParameters", "%D Períodos");
            DictionarySelectIndicator.Add("lblDType", "%D MA Tipo");
            DictionarySelectIndicator.Add("lblKSmooth", "%KSmooth");
            DictionarySelectIndicator.Add("lblKDblSmooth", "%K Dbl Smooth");
            DictionarySelectIndicator.Add("lblTemplates", "Modelos");
            DictionarySelectIndicator.Add("lblDefault", "Padrão");
            DictionarySelectIndicator.Add("lblRename", "Renomear");
            DictionarySelectIndicator.Add("lblVariation", "Variação(%)");
            DictionarySelectIndicator.Add("lblThreshold1", "Limite Inferior");
            DictionarySelectIndicator.Add("lblThreshold2", "Limite Superior");
            DictionarySelectIndicator.Add("pgrdIndicators", "Parâmetros");
            DictionarySelectIndicator.Add("gprAverage", "Média");
            DictionarySelectIndicator.Add("gprValue", "Valor");
            DictionarySelectIndicator.Add("gprWindow", "Janela");
            DictionarySelectIndicator.Add("gprHistogram", "Histograma");
            DictionarySelectIndicator.Add("gprParameters", "Parâmetros");
            DictionarySelectIndicator.Add("gprKParameters", "%K Parâmetros");
            DictionarySelectIndicator.Add("gprDParameters", "%D Parâmetros");
            DictionarySelectIndicator.Add("gprView", "Aparência");
            DictionarySelectIndicator.Add("gprThresholds", "Limites");
            // Indicator's names
            DictionarySelectIndicator.Add("MA", "MM");
            DictionarySelectIndicator.Add("Moving Average", "MM Média Móvel");
            DictionarySelectIndicator.Add("MACD", "MACD");
            DictionarySelectIndicator.Add("MACD Histogram", "MACD Histograma");
            DictionarySelectIndicator.Add("Volume", "Volume");
            DictionarySelectIndicator.Add("Moving Average Envelope", "MME Média M. Envelope");
            DictionarySelectIndicator.Add("Bollinger Bands", "BB Bandas de Bollinger");
            DictionarySelectIndicator.Add("Relative Strength Index", "IFR Índice de Força Relativa");
            DictionarySelectIndicator.Add("HILO Activator", "HILO Activator");
			DictionarySelectIndicator.Add("Typical Price", "PM Preço Médio");
            DictionarySelectIndicator.Add("Parabolic SAR", "PSAR Parabólico SAR");
            DictionarySelectIndicator.Add("Stochastic Momentum Index", "ME Momento Estocástico");
            DictionarySelectIndicator.Add("Stochastic Oscillator", "OE Oscilador Estocástico");
            DictionarySelectIndicator.Add("Aroon Oscillator", "AO Aroon Oscilador");
            DictionarySelectIndicator.Add("Money Flow Index", "IFD Índice do Fluxo de Dinheiro");
            DictionarySelectIndicator.Add("Simple Moving Average", "MMS Média Móvel Simples");
            DictionarySelectIndicator.Add("Accumulative Swing Index", "IBA Índice de Balanço Acumulativo");
            DictionarySelectIndicator.Add("Exponential Moving Average", "EMM Média Móvel Exponencial");
            DictionarySelectIndicator.Add("Chaikin Volatility", "VC Volatilidade Chaikin");
            DictionarySelectIndicator.Add("Historical Volatility", "VH Volatilidade Histórica");
            DictionarySelectIndicator.Add("Chande Momentum Oscillator", "CMO Chande Momentum Oscillator");
            DictionarySelectIndicator.Add("Welles Wilder Smoothing", "AWW Alinhamento Welles Wilder");
            DictionarySelectIndicator.Add("Vertical Horizontal Filter", "FVH Filtro Vertical Horizontal");
            DictionarySelectIndicator.Add("Linear Regression R-Squared", "RLRR Regressão Linear Raiz-R");
            DictionarySelectIndicator.Add("Linear Regression Forecast", "PRL Previsão de Regressão Linear");
            DictionarySelectIndicator.Add("Linear Regression Slope", "IRL Inclinação da Regressão Linear");
            DictionarySelectIndicator.Add("Linear Regression Intercept", "RLI Regressão Linear Interseção");
            DictionarySelectIndicator.Add("Momentum Oscillator", "MO Momentum Oscillator");
            DictionarySelectIndicator.Add("ADX", "ADX");
            DictionarySelectIndicator.Add("DI+/DI-", "DI+/DI-");
            DictionarySelectIndicator.Add("DI+/DI-/ADX", "DI+/DI-/ADX");
            DictionarySelectIndicator.Add("Commodity Channel Index", "CCI Commodity Channel Index");
            DictionarySelectIndicator.Add("Detrended Price Oscillator", "DPO Detrended Price Oscillator");
            DictionarySelectIndicator.Add("Mass Index", "IM Índice de Massa");
            DictionarySelectIndicator.Add("On Balance Volume", "OBV On Balance Volume");
            DictionarySelectIndicator.Add("Price ROC", "PROC Preço ROC");
            DictionarySelectIndicator.Add("Price Volume Trend", "TVP Tendência do Volume e Preço");
            DictionarySelectIndicator.Add("Williams %R", "Williams %R");
            DictionarySelectIndicator.Add("Williams Accumulation Distribution", "WAD Williams Accumulation Distribution");
            DictionarySelectIndicator.Add("Wheighted Close", "WC Wheighted Close");
            DictionarySelectIndicator.Add("Volume ROC", "VROC Volume ROC");
            DictionarySelectIndicator.Add("Fractal Chaos Oscillator", "FCO Fractal Chaos Oscillator");
            DictionarySelectIndicator.Add("Fractal Chaos Bands", "FCB Fractal Chaos Bands");
            DictionarySelectIndicator.Add("Ultimate Oscillator", "UO Ultimate Oscillator");
            DictionarySelectIndicator.Add("True Range", "TR True Range");
            DictionarySelectIndicator.Add("Rainbow Oscillator", "RO Rainbow Oscillator");
            DictionarySelectIndicator.Add("Accumulation/Distribution", "A/D Acumulação Distribuição");
        }

        public override sealed void LoadSelectStudy()
        {
            DictionarySelectIndicator.Add("FrmSelectStudyTitle", "Estudos");
            DictionarySelectIndicator.Add("lblToolsStudy", "Ferramentas");
            DictionarySelectIndicator.Add("gprStudyColor", "Cor");
            DictionarySelectIndicator.Add("gprStudyParameters", "Paramêtros");
            DictionarySelectIndicator.Add("gprStudyLineStyle", "Estilo da Linha");
            DictionarySelectIndicator.Add("gprStudyLineThickness", "Espessura da Linha");
            DictionarySelectIndicator.Add("gprStudyRightExtension", "Extensão Direita");
            DictionarySelectIndicator.Add("gprStudyLeftExtension", "Extensão Esquerda");
            DictionarySelectIndicator.Add("gprStudyExtension", "Extensão");
            DictionarySelectIndicator.Add("gprStudyValue", "Valor");
            DictionarySelectIndicator.Add("StudyLine", "Linha de Estudos");
            DictionarySelectIndicator.Add("TrendLine", "Linha de Tendência");
            DictionarySelectIndicator.Add("RayLine", "Linha de Raio");
            DictionarySelectIndicator.Add("VerticalLine", "Linha Vertical");
            DictionarySelectIndicator.Add("HorizontalLine", "Linha Horizontal");
            DictionarySelectIndicator.Add("Rectangle", "Retângulo");
            DictionarySelectIndicator.Add("Ellipse", "Elipse");
            DictionarySelectIndicator.Add("Arrow", "Seta");
            DictionarySelectIndicator.Add("FreeHand", "Desenho Livre");
            DictionarySelectIndicator.Add("Polyline", "Polyline");
            DictionarySelectIndicator.Add("Channel", "Canal");
            DictionarySelectIndicator.Add("Fibonacci Arcs", "Arcos de Fibonacci");
            DictionarySelectIndicator.Add("Fibonacci Fan", "Fibonacci Fan");
            DictionarySelectIndicator.Add("Fibonacci Projections", "Projeções Fibonacci");
            DictionarySelectIndicator.Add("Fibonacci Retracements", "Retrações Fibonacci");
            DictionarySelectIndicator.Add("Fibonacci Timezones", "Zonas de Tempo Fibonacci");
            DictionarySelectIndicator.Add("Speed Line", "Linha de Velocidade");
            DictionarySelectIndicator.Add("Gann Fan", "Gann Fan");
        }

        public override sealed void LoadBrowser()
        {
            DictionaryBrowser.Add("tsbUndo", "Voltar");
            DictionaryBrowser.Add("tsbRedo", "Avançar");
            DictionaryBrowser.Add("tsbRefresh", "Atualizar");
            DictionaryBrowser.Add("tsbPageHome", "Página Principal");
            DictionaryBrowser.Add("tsbNavigate", "Ir para");
        }

        public override sealed void LoadSelectTools()
        {
            DictionarySelectTools.Add("newChart", "Abrir Novo Gráfico");
            DictionarySelectTools.Add("editPortfolios", "Editar Portfolios");
            DictionarySelectTools.Add("viewChart", "Abrir Gráfico");
            DictionarySelectTools.Add("columnSymbol", "Ativo");
            DictionarySelectTools.Add("columnLast", "Último");
            DictionarySelectTools.Add("columnVariation", "Variação");
            DictionarySelectTools.Add("columnVolume", "Volume");
            DictionarySelectTools.Add("columnTime", "Data/Hora");
            DictionarySelectTools.Add("columnOpen", "Abertura");
            DictionarySelectTools.Add("columnHigh", "Máximo");
            DictionarySelectTools.Add("columnLow", "Mínimo");
            DictionarySelectTools.Add("columnClose", "Fechamento");
            DictionarySelectTools.Add("columnTrades", "Negócios");
            DictionarySelectTools.Add("selectTools", "Seletor de Ativos");
            DictionarySelectTools.Add("tabDetails", "Detalhes");
            DictionarySelectTools.Add("tabPerformance", "Volume");
            DictionarySelectTools.Add("tabAll", "Todos");
        }

        public override sealed void LoadSettings()
        {
            DictionarySettings.Add("frmOptions", "Opções");
            DictionarySettings.Add("msgSaved", "Alterações salvas com sucesso!");

            //Button's
            DictionarySettings.Add("btnApply", "Aplicar");
            DictionarySettings.Add("btnOk", "Ok");
            DictionarySettings.Add("btnCancel", "Cancelar");
            DictionarySettings.Add("cbApplyAll", "Aplicar em todos os gráficos");

            //Chart
            DictionarySettings.Add("grpBehavior", "Comportamento");
            DictionarySettings.Add("grpAppearance", "Aparência");
            DictionarySettings.Add("grpNumberCandles", "Número de Candles");
            DictionarySettings.Add("grpTabData", "Aba de Dados");

            DictionarySettings.Add("tabChart", "Gráfico");
            DictionarySettings.Add("cbxSemiLogScale", "Escala Logaritmica");
            DictionarySettings.Add("cbxPanelSeparator", "Separador de Painéis");
            DictionarySettings.Add("cbxGridVertical", "Gride Vertical");
            DictionarySettings.Add("cbxGridHorizontal", "Gride Horizontal");
            DictionarySettings.Add("lblPaddingTop", "Espaçamento Acima");
            DictionarySettings.Add("lblPaddingBottom", "Espaçamento Abaixo");
            DictionarySettings.Add("lblPaddingRight", "Espaçamento à Direita");
            DictionarySettings.Add("lblChartViewport", "Barras em Exibição");
            DictionarySettings.Add("lblDecimals", "Casas Decimais");
            DictionarySettings.Add("lblChartHistory", "Barras em Histórico");
            DictionarySettings.Add("lblPositionChart", "Posição");
            DictionarySettings.Add("grpChart", "Configurações");
            DictionarySettings.Add("lblColorScheme", "Esquema de Cores");
            DictionarySettings.Add("cbxVisiblePortfolio", "Portfolio");

            //TabData
            DictionarySettings.Add("LessChart", "Lateral");
            DictionarySettings.Add("BottomChart", "Inferior");

            //Colors Scheme 
            DictionarySettings.Add("SchemeSky", "Suave");
            DictionarySettings.Add("SchemeWhite", "Branco");
            DictionarySettings.Add("SchemeBlue", "Azul");
            DictionarySettings.Add("SchemeBeige", "Bege");
            DictionarySettings.Add("SchemeDark", "Escuro");
            DictionarySettings.Add("SchemeGreen", "Verde");
            DictionarySettings.Add("SchemeMono", "Mono");

            //Studies
            DictionarySettings.Add("tabStudies", "Estudos");
            DictionarySettings.Add("grpStudies", "Configurações");
            DictionarySettings.Add("lblLineThickness", "Largura da Linha");
            DictionarySettings.Add("lblColor", "Cor");
            DictionarySettings.Add("columnRetracements", "Retrações");
            DictionarySettings.Add("columnProjections", "Projeções");
            DictionarySettings.Add("lblFibonacci", "Fibonacci");

            //Color
            DictionarySettings.Add("btnAssociate", "Bloquear/Desbloquear Fundo - Gradiente");
            DictionarySettings.Add("tabColor", "Cores");
            DictionarySettings.Add("grpColors", "Configurações");
            DictionarySettings.Add("lblBackGradientBottom", "Fundo - Gradiente Inferior");
            DictionarySettings.Add("lblBackGradientTop", "Fundo - Gradiente Superior");
            DictionarySettings.Add("lblGridColor", "Gride");
            DictionarySettings.Add("lblDownColor", "Candle de Baixa");
            DictionarySettings.Add("lblUpColor", "Candle de Alta");
            DictionarySettings.Add("lblScale", "Escala");
            DictionarySettings.Add("lblChartBackColor", "Fundo do Gráfico");
            DictionarySettings.Add("lblPainelSeparatorColor", "Separador de Painéis");
            DictionarySettings.Add("lblCandleBorderColor", "Contorno dos Candles");
            DictionarySettings.Add("cbxOverwriteAll", "Sobrescrever Todos");
            DictionarySettings.Add("grpOverwrite", "Sobrescrever Tema");

            //Server
            DictionarySettings.Add("tabServer", "Servidor");
            DictionarySettings.Add("lblPort1", "Porta 1");
            DictionarySettings.Add("lblPort2", "Porta 2");
            DictionarySettings.Add("lblPort3", "Porta 3");
            DictionarySettings.Add("lblServer1", "Servidor 1");
            DictionarySettings.Add("lblServer2", "Servidor 2");
            DictionarySettings.Add("lblServer3", "Servidor 3");
            DictionarySettings.Add("grpSettingsServer", "Configurações");

            //Proxy
            DictionarySettings.Add("tabProxy", "Conexão");
            DictionarySettings.Add("grpSettingsProxy", "Configurações");
            DictionarySettings.Add("optNotProxy", "Não usar servidor de proxy");
            DictionarySettings.Add("optConfigProxyNavigator", "Usar as configurações de proxy do navegador da Web");
            DictionarySettings.Add("optProxyServer", "Usar o seguinte servidor de proxy");
            DictionarySettings.Add("Address", "Endereço");
            DictionarySettings.Add("Port", "Porta");
            DictionarySettings.Add("optProxySocks", "Usar o seguinte proxy SOCKS");
            DictionarySettings.Add("cbxAuthenticationProxy", "Requer Autenticação");
            DictionarySettings.Add("lblUser", "Usuário");
            DictionarySettings.Add("lblPassword", "Senha");

            //Price
            DictionarySettings.Add("tabPrice", "Preços");
            DictionarySettings.Add("grpPrice", "Configurações");
            DictionarySettings.Add("grpLine", "Linha");
            DictionarySettings.Add("grpBar", "Barra");
            DictionarySettings.Add("grpSmoothed", "HA Suavizado");
            DictionarySettings.Add("lblMonoColor", "Mono/Colorida");
            DictionarySettings.Add("lblLineThicknessLine", "Espessura da Linha");
            DictionarySettings.Add("lblLineThicknessBar", "Espessura da Linha");
            DictionarySettings.Add("lblPeriodsSmoothed", "Períodos");
            DictionarySettings.Add("lblTypeAverageSmoothed", "Tipo de Média");

            //User
            DictionarySettings.Add("tabUser", "Usuário");
            DictionarySettings.Add("grpBasic", "Cadastro");
            DictionarySettings.Add("grpOptional", "Informações");
            DictionarySettings.Add("lblFirstName", "Nome");
            DictionarySettings.Add("lblLastName", "Sobrenome");
            DictionarySettings.Add("lblCPF", "CPF");
            DictionarySettings.Add("lblEmail", "Email");
            DictionarySettings.Add("lblUserName", "Login");
            DictionarySettings.Add("lblOldPassword", "Senha Antiga");
            DictionarySettings.Add("lblNewPassword", "Nova Senha");
            DictionarySettings.Add("lblNewPassword2", "Confirmar Nova Senha");
            DictionarySettings.Add("lblBirthday", "Nascimento");
            DictionarySettings.Add("lblCEP", "CEP");
            DictionarySettings.Add("lblState", "Estado");
            DictionarySettings.Add("lblCity", "Cidade");
            DictionarySettings.Add("lblDistrict", "Bairro");
            DictionarySettings.Add("lblStreet", "Lagradouro");
            DictionarySettings.Add("lblNumber", "Número");
            DictionarySettings.Add("lblComplement", "Complemento");
        }

        public override sealed void LoadPortfolio()
        {
            DictionaryPortfolio.Add("lblPortfolio", "Portfolio");
            DictionaryPortfolio.Add("PortfolioExists", "não existente, favor digitar um portfolio válido!");
            DictionaryPortfolio.Add("fieldEmptyPortfolio", "O portfolio informado não pode ser vazio");
            DictionaryPortfolio.Add("frmEditPortfolio", "Editar Portfolio");
            DictionaryPortfolio.Add("toolTipAddPortfolio", "Novo Portfolio");
            DictionaryPortfolio.Add("toolTipDeletePortfolio", "Remover Portfolio");
            DictionaryPortfolio.Add("lblSearchPort", "Procurar");
            DictionaryPortfolio.Add("toolTipAddPortList", "Adicionar");
            DictionaryPortfolio.Add("toolTipRemovePortList", "Remover");
            DictionaryPortfolio.Add("btnOk", "Ok");
            DictionaryPortfolio.Add("btnApply", "Aplicar");
            DictionaryPortfolio.Add("btnCancel", "Cancelar");
            DictionaryPortfolio.Add("ConfirmSavePortfolio", "Salvar alterações em");
            DictionaryPortfolio.Add("ConfirmDeletePortfolio", "Confirma exclusão do portfolio?");
            DictionaryPortfolio.Add("portfolioExisted", "O nome {0} tem sido usada já em outra carteira!");
            DictionaryPortfolio.Add("portfolioCreated", "Nova carteira {0} criado!");
            DictionaryPortfolio.Add("portfolioRenameExist", "Este nome tem sido usado já em outra carteira!");
            DictionaryPortfolio.Add("portfolioRenameInvalid", "Este nome é inválido!");
            DictionaryPortfolio.Add("FormPortfolio", "Novo Portfolio");
            DictionaryPortfolio.Add("FormRanameTab", "Renomear");
        }

        public override sealed void LoadLogin()
        {
            DictionaryLogin.Add("frmLogin", "Plena Login");
            DictionaryLogin.Add("lblLogin", "Login");
            DictionaryLogin.Add("lblPassword", "Senha");
            DictionaryLogin.Add("btnGuest", "Entrar como convidado");
            DictionaryLogin.Add("lblForgotPassword", "Login/Senha esquecidos");
            DictionaryLogin.Add("lblRegister", "Registrar");
            DictionaryLogin.Add("btnOk", "Ok");
            DictionaryLogin.Add("invalidLogin", "Login/Senha inválidos");
            DictionaryLogin.Add("blockedLogin", "Sistema bloqueado para acesso.");
            DictionaryLogin.Add("txtLoginEmpty", "Informe o campo login");
            DictionaryLogin.Add("txtPasswordEmpty", "Informe o campo senha");
            DictionaryLogin.Add("notConnectServer", "Não foi possível conectar ao servidor. Tente novamente mais tarde.");
            DictionaryLogin.Add("lblVersion", "Versão Beta 2014");
            DictionaryLogin.Add("lblStatus", "Informe seu login e senha...");
            DictionaryLogin.Add("cbxLembrar", "Lembrar");
            DictionaryLogin.Add("cbxOffline", "Entrar Offline");
            DictionaryLogin.Add("messageLogin", "Usuário {0} em modo {1}.");
            DictionaryLogin.Add("ServerValidate", "Validando login e senha no servidor.");
            DictionaryLogin.Add("ValidationSuccessful", "Validação realizada com sucesso.");
            DictionaryLogin.Add("InsertLogin", "Informe seu login e senha...");
            DictionaryLogin.Add("OpeningDB", "Abrindo base de dados...");
            DictionaryLogin.Add("ImpossibleAtMoment", "Não foi possível neste momento...");
            DictionaryLogin.Add("ImpossibleAccessDB", "Não foi possível conectar ao banco de dados e a aplicação será encerrada. Caso este erro persista entre em contato com suporte@plena-tp.com.br.");
            DictionaryLogin.Add("LoadingDB", "Carregando base de dados...");
            DictionaryLogin.Add("LoadingChartCommands", "Carregando chart da linha de comandos...");
            DictionaryLogin.Add("LoadingDM", "Carregando DataManager...");
            DictionaryLogin.Add("CheckingWorkspace", "Verificando workspace...");
            DictionaryLogin.Add("LoadWebBrowser", "Carregando o Web Browser...");
            DictionaryLogin.Add("LoadLastWorkspace", "Carregando último workspace salvo...");
            DictionaryLogin.Add("msgGuestInfo", @"O modo Convidado te permite conhecer o funcionamento da plataforma sem maiores burocracias. 
Como restrições, o  modo Convidado não permite a atualização da base de dados e lhe dá acesso 
à visualização dos últimos 300 candles de dados. O modo Convidado ficará válido por 3 dias após 
a instalação do sistema. Após esse período, o acesso ao sistema só será possível mediante usuário registrado.

Aguardamos por seu registro!");
            DictionaryLogin.Add("msgGuestInvalid", @"O modo Convidado só é válido por 3 dias após a instalação do sistema. 
Faça o seu registro e tenha acesso ao Plena Trading Platform.");
            DictionaryLogin.Add("msgGuestError", "Valor inválido para convidados!");
            DictionaryLogin.Add("msgSymbolGuest", "Convidados não têm acesso ao ativo ");
        }

        public override sealed void LoadPermission()
        {
            DictionaryPermission.Add("unauthorizedAccess", "Acesso não permitido");
        }

        public override sealed void LoadTemplate()
        {
            DictionaryTemplate.Add("frmTemplate", "Modelos");
            DictionaryTemplate.Add("lblDescription", "Descrição");
            DictionaryTemplate.Add("cbxDefault", "Padrão");
        }

        public override sealed void LoadWorkspace()
        {
            DictionaryWorkspace.Add("workspace", "Área de Trabalho");
            DictionaryWorkspace.Add("frmWorkspace", "Área de Trabalho");
            DictionaryWorkspace.Add("btnDismiss", "Fechar");
            DictionaryWorkspace.Add("mnuLoadWorkspace", "Carregar"); 
            DictionaryWorkspace.Add("mnuDefaultWorkspace", "Default");
            DictionaryWorkspace.Add("mnuRenameWorkspace", "Renomear");
        }

        public override sealed void LoadOutput()
        {
            DictionaryOutput.Add("tabConnection", "Conexão");
            DictionaryOutput.Add("tabAlerts", "Alertas & Mensagens");
        }
    }
}