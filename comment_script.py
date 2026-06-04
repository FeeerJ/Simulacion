import os

files_to_comment = {
    'Simulacion.Application/PlantServices/SegmentacionService.cs': {
        'public ResultadoSegmentacion ProcesoSegmentar(int aptos)': '        // Segmenta los dispositivos aptos en diferentes tipos (CRT, LCD, LED) y aparta los de refurbishment\n        public ResultadoSegmentacion ProcesoSegmentar(int aptos)'
    },
    'Simulacion.Application/PlantServices/ExtraccionMaterialesService.cs': {
        'public ResultadoExtraccion ExtraerMateriales(int cantCRT, int cantLCD, int cantLED)': '        // Simula la extraccion de materiales valiosos y toxicos de cada lote procesado\n        public ResultadoExtraccion ExtraerMateriales(int cantCRT, int cantLCD, int cantLED)'
    },
    'Simulacion.Application/PlantServices/PesoService.cs': {
        'public ResultadoPeso CalcularPeso(int cantCRT, int cantLCD, int cantLED)': '        // Calcula el peso total de los dispositivos basados en sus distribuciones estadisticas\n        public ResultadoPeso CalcularPeso(int cantCRT, int cantLCD, int cantLED)'
    },
    'Simulacion.Application/PlantServices/SustanciasToxicasService.cs': {
        'public ResultadoSustanciasToxicas ProcesarSustancias(double pesoTotalLote)': '        // Determina la proporcion toxica y calcula el costo asociado a su disposicion segura\n        public ResultadoSustanciasToxicas ProcesarSustancias(double pesoTotalLote)'
    },
    'Simulacion.Application/PlantServices/DistributionService.cs': {
        'public double GenerarBinomial(double n, double p)': '        // Genera un valor siguiendo la distribucion Binomial (util para proporciones)\n        public double GenerarBinomial(double n, double p)'
    },
    'Simulacion.Application/Services/CongruentialMethodService.cs': {
        'public List<double> GenerateNumbers(int x0, int a, int c, int m, int n)': '        // Generador congruencial lineal de numeros pseudoaleatorios\n        public List<double> GenerateNumbers(int x0, int a, int c, int m, int n)'
    },
    'Simulacion.Application/Services/LehmerService.cs': {
        'public List<double> GenerateNumbers(int x0, int k, int m, int c, int n)': '        // Algoritmo de Lehmer para generacion de numeros pseudoaleatorios\n        public List<double> GenerateNumbers(int x0, int k, int m, int c, int n)'
    },
    'Simulacion.Application/Services/MidSquareService.cs': {
        'public List<double> GenerateNumbers(int seed, int n)': '        // Metodo de los cuadrados medios para generar valores pseudoaleatorios\n        public List<double> GenerateNumbers(int seed, int n)'
    },
    'Simulacion.Application/Services/StatisticalTestsService.cs': {
        'public Dictionary<string, object> RunChiSquareTest(List<double> data, int numIntervals)': '        // Ejecuta pruebas estadisticas (Chi-Cuadrado, Kolmogorov-Smirnov) para validar la uniformidad\n        public Dictionary<string, object> RunChiSquareTest(List<double> data, int numIntervals)'
    }
}

for file_path, replacements in files_to_comment.items():
    if os.path.exists(file_path):
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        for target, replacement in replacements.items():
            content = content.replace(target, replacement)
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"Comentado: {file_path}")
    else:
        print(f"No encontrado: {file_path}")
