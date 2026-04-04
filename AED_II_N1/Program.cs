using System.Globalization;
using System.Text;

// MODELOS 
class Aluno
{
    public int Matricula { get; set; }
    public string Nome { get; set; } = "";
    public int Idade { get; set; }
}

class Disciplina
{
    public int Codigo { get; set; }
    public string Nome { get; set; } = "";
    public double NotaMinima { get; set; }
}

class Matricula
{
    public int CodDisciplina { get; set; }
    public int MatriculaAluno { get; set; }
    public double? Nota1 { get; set; }
    public double? Nota2 { get; set; }
    public double? Media => (Nota1.HasValue && Nota2.HasValue) ? (Nota1.Value + Nota2.Value) / 2.0 : null;
}

//  PROGRAMA
class Program
{
    static List<Aluno> alunos = new();
    static List<Disciplina> disciplinas = new();
    static List<Matricula> matriculas = new();

    static readonly string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
    static readonly string AlunosFile = Path.Combine(BaseDir, "Alunos.dat");
    static readonly string DisciplinasFile = Path.Combine(BaseDir, "Disciplinas.dat");
    static readonly string MatriculasFile = Path.Combine(BaseDir, "Matriculas.dat");
    static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;
    static readonly Encoding UTF8 = new UTF8Encoding(false);

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        CarregarArquivos();

        bool rodando = true;
        while (rodando)
        {
            LimparTela();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║   Sistema de Gestão de Provas    ║");
            Console.WriteLine("╠══════════════════════════════════╣");
            Console.WriteLine("║  1. Consultas                    ║");
            Console.WriteLine("║  2. Cadastros                    ║");
            Console.WriteLine("║  3. Salvar                       ║");
            Console.WriteLine("║  4. Sair                         ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.Write("Opção: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": MenuConsultas(); break;
                case "2": MenuCadastros(); break;
                case "3":
                    SalvarArquivos();
                    Console.WriteLine("\nDados salvos com sucesso!");
                    Pausar();
                    break;
                case "4":
                    SalvarArquivos();
                    rodando = false;
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    Pausar();
                    break;
            }
        }
        Console.WriteLine("Programa encerrado.");
    }

    // LEITURA E GRAVAÇÃO DOS ARQUIVOS 

    static void CarregarArquivos()
    {
        if (File.Exists(AlunosFile))
        {
            foreach (var linha in File.ReadAllLines(AlunosFile, UTF8))
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;
                var p = linha.Split(';');
                if (p.Length >= 3 &&
                    int.TryParse(p[0], out int mat) &&
                    int.TryParse(p[2], out int idade))
                    alunos.Add(new Aluno { Matricula = mat, Nome = p[1], Idade = idade });
            }
        }

        if (File.Exists(DisciplinasFile))
        {
            foreach (var linha in File.ReadAllLines(DisciplinasFile, UTF8))
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;
                var p = linha.Split(';');
                if (p.Length >= 3 &&
                    int.TryParse(p[0], out int cod) &&
                    double.TryParse(p[2], NumberStyles.Any, Invariant, out double nm))
                    disciplinas.Add(new Disciplina { Codigo = cod, Nome = p[1], NotaMinima = nm });
            }
        }

        if (File.Exists(MatriculasFile))
        {
            foreach (var linha in File.ReadAllLines(MatriculasFile, UTF8))
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;
                var p = linha.Split(';');
                if (p.Length >= 2 &&
                    int.TryParse(p[0], out int cod) &&
                    int.TryParse(p[1], out int mat))
                {
                    double? n1 = p.Length > 2 && double.TryParse(p[2], NumberStyles.Any, Invariant, out double v1) ? v1 : null;
                    double? n2 = p.Length > 3 && double.TryParse(p[3], NumberStyles.Any, Invariant, out double v2) ? v2 : null;
                    matriculas.Add(new Matricula { CodDisciplina = cod, MatriculaAluno = mat, Nota1 = n1, Nota2 = n2 });
                }
            }
        }
    }

    static void SalvarArquivos()
    {
        File.WriteAllLines(AlunosFile,
            alunos.Select(a => $"{a.Matricula};{a.Nome};{a.Idade}"), UTF8);

        File.WriteAllLines(DisciplinasFile,
            disciplinas.Select(d => $"{d.Codigo};{d.Nome};{d.NotaMinima.ToString(Invariant)}"), UTF8);

        File.WriteAllLines(MatriculasFile,
            matriculas.Select(m =>
                $"{m.CodDisciplina};{m.MatriculaAluno};" +
                $"{(m.Nota1.HasValue ? m.Nota1.Value.ToString(Invariant) : "")};" +
                $"{(m.Nota2.HasValue ? m.Nota2.Value.ToString(Invariant) : "")}"), UTF8);
    }

    // MENUS 

    static void MenuConsultas()
    {
        bool loop = true;
        while (loop)
        {
            LimparTela();
            Console.WriteLine("=== Consultas ===");
            Console.WriteLine("  1.   Alunos");
            Console.WriteLine("  2.  Disciplinas");
            Console.WriteLine("  3. Alunos da Disciplina");
            Console.WriteLine("  4.  Disciplinas do Aluno");
            Console.WriteLine("  0.   Voltar");
            Console.Write("Opção: ");

            switch (Console.ReadLine()?.Trim().ToLower())
            {
                case "i":   case "1": ConsultarAlunos(); break;
                case "ii":  case "2": ConsultarDisciplinas(); break;
                case "iii": case "3": ConsultarAlunosDaDisciplina(); break;
                case "iv":  case "4": ConsultarDisciplinasDoAluno(); break;
                case "0": loop = false; break;
                default: Console.WriteLine("Opção inválida!"); Pausar(); break;
            }
        }
    }

    static void MenuCadastros()
    {
        bool loop = true;
        while (loop)
        {
            LimparTela();
            Console.WriteLine("=== Cadastros ===");
            Console.WriteLine("  1.   Alunos");
            Console.WriteLine("  2.  Disciplinas");
            Console.WriteLine("  3. Matrículas");
            Console.WriteLine("  4.  Atribuir Nota a Aluno");
            Console.WriteLine("  0.   Voltar");
            Console.Write("Opção: ");

            switch (Console.ReadLine()?.Trim().ToLower())
            {
                case "i":   case "1": CadastrarAluno(); break;
                case "ii":  case "2": CadastrarDisciplina(); break;
                case "iii": case "3": CadastrarMatricula(); break;
                case "iv":  case "4": AtribuirNota(); break;
                case "0": loop = false; break;
                default: Console.WriteLine("Opção inválida!"); Pausar(); break;
            }
        }
    }

    // CONSULTAS 

    static void ConsultarAlunos()
    {
        LimparTela();
        Console.WriteLine("=== Lista de Alunos ===\n");

        if (alunos.Count == 0)
        {
            Console.WriteLine("Nenhum aluno cadastrado.");
        }
        else
        {
            Console.WriteLine($"{"Matrícula",-12} {"Nome",-30} Idade");
            Console.WriteLine(new string('-', 52));
            foreach (var a in alunos)
                Console.WriteLine($"{a.Matricula,-12} {a.Nome,-30} {a.Idade}");
        }
        Pausar();
    }

    static void ConsultarDisciplinas()
    {
        LimparTela();
        Console.WriteLine("=== Lista de Disciplinas ===\n");

        if (disciplinas.Count == 0)
        {
            Console.WriteLine("Nenhuma disciplina cadastrada.");
        }
        else
        {
            Console.WriteLine($"{"Código",-8} {"Nome",-30} Nota Mínima");
            Console.WriteLine(new string('-', 52));
            foreach (var d in disciplinas)
                Console.WriteLine($"{d.Codigo,-8} {d.Nome,-30} {d.NotaMinima}");
        }
        Pausar();
    }

    static void ConsultarAlunosDaDisciplina()
    {
        LimparTela();
        Disciplina? disc = null;
        while (disc == null)
        {
            Console.Write("Nome ou código da disciplina (0 para voltar): ");
            string entrada = Console.ReadLine()?.Trim() ?? "";
            if (entrada == "0") return;
            disc = BuscarDisciplina(entrada);
            if (disc == null) Console.WriteLine("Disciplina não encontrada. Tente novamente.\n");
        }

        LimparTela();
        Console.WriteLine($"=== Alunos da Disciplina: {disc.Nome} | Nota Mínima: {disc.NotaMinima} ===\n");

        var mats = matriculas.Where(m => m.CodDisciplina == disc.Codigo).ToList();
        if (mats.Count == 0)
        {
            Console.WriteLine("Nenhum aluno matriculado nesta disciplina.");
        }
        else
        {
            Console.WriteLine($"{"Nome",-28} {"Nota1",-8} {"Nota2",-8} {"Média",-8} Situação");
            Console.WriteLine(new string('-', 68));
            foreach (var m in mats)
            {
                var aluno = alunos.FirstOrDefault(a => a.Matricula == m.MatriculaAluno);
                string nome = aluno?.Nome ?? "(desconhecido)";
                string n1   = m.Nota1.HasValue ? m.Nota1.Value.ToString("F1") : "-";
                string n2   = m.Nota2.HasValue ? m.Nota2.Value.ToString("F1") : "-";
                string med  = m.Media.HasValue  ? m.Media.Value.ToString("F1") : "-";
                string sit  = m.Media.HasValue
                    ? (m.Media.Value >= disc.NotaMinima ? "Aprovado" : "Reprovado")
                    : "Sem notas";
                Console.WriteLine($"{nome,-28} {n1,-8} {n2,-8} {med,-8} {sit}");
            }
        }
        Pausar();
    }

    static void ConsultarDisciplinasDoAluno()
    {
        LimparTela();
        Aluno? aluno = null;
        while (aluno == null)
        {
            Console.Write("Nome ou matrícula do aluno (0 para voltar): ");
            string entrada = Console.ReadLine()?.Trim() ?? "";
            if (entrada == "0") return;
            aluno = BuscarAluno(entrada);
            if (aluno == null) Console.WriteLine("Aluno não encontrado. Tente novamente.\n");
        }

        LimparTela();
        Console.WriteLine($"=== Disciplinas do Aluno: {aluno.Nome} ===\n");

        var mats = matriculas.Where(m => m.MatriculaAluno == aluno.Matricula).ToList();
        if (mats.Count == 0)
        {
            Console.WriteLine("Aluno não está matriculado em nenhuma disciplina.");
        }
        else
        {
            Console.WriteLine($"{"Disciplina",-28} {"Nota1",-8} {"Nota2",-8} {"Média",-8} Situação");
            Console.WriteLine(new string('-', 68));
            foreach (var m in mats)
            {
                var disc = disciplinas.FirstOrDefault(d => d.Codigo == m.CodDisciplina);
                string nomeDisc = disc?.Nome ?? "(desconhecida)";
                string n1  = m.Nota1.HasValue ? m.Nota1.Value.ToString("F1") : "-";
                string n2  = m.Nota2.HasValue ? m.Nota2.Value.ToString("F1") : "-";
                string med = m.Media.HasValue  ? m.Media.Value.ToString("F1") : "-";
                string sit = m.Media.HasValue && disc != null
                    ? (m.Media.Value >= disc.NotaMinima ? "Aprovado" : "Reprovado")
                    : "Sem notas";
                Console.WriteLine($"{nomeDisc,-28} {n1,-8} {n2,-8} {med,-8} {sit}");
            }
        }
        Pausar();
    }

    // CADASTROS

    static void CadastrarAluno()
    {
        LimparTela();
        Console.WriteLine("=== Cadastro de Aluno ===\n");

        Console.Write("Nome: ");
        string nome = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Idade: ");
        if (!int.TryParse(Console.ReadLine()?.Trim(), out int idade))
        {
            Console.WriteLine("Idade inválida!");
            Pausar();
            return;
        }

        int novaMatricula = alunos.Count > 0 ? alunos.Max(a => a.Matricula) + 1 : 1;
        alunos.Add(new Aluno { Matricula = novaMatricula, Nome = nome, Idade = idade });
        Console.WriteLine($"\nAluno cadastrado com sucesso! Matrícula: {novaMatricula}");
        Pausar();
    }

    static void CadastrarDisciplina()
    {
        LimparTela();
        Console.WriteLine("=== Cadastro de Disciplina ===\n");

        Console.Write("Nome: ");
        string nome = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Nota Mínima: ");
        if (!TryParseDouble(Console.ReadLine(), out double notaMin))
        {
            Console.WriteLine("Nota mínima inválida!");
            Pausar();
            return;
        }

        int novoCodigo = disciplinas.Count > 0 ? disciplinas.Max(d => d.Codigo) + 1 : 1;
        disciplinas.Add(new Disciplina { Codigo = novoCodigo, Nome = nome, NotaMinima = notaMin });
        Console.WriteLine($"\nDisciplina cadastrada com sucesso! Código: {novoCodigo}");
        Pausar();
    }

    static void CadastrarMatricula()
    {
        LimparTela();
        Console.WriteLine("=== Cadastro de Matrícula ===\n");

        Aluno? aluno = null;
        while (aluno == null)
        {
            Console.Write("Nome ou matrícula do aluno (0 para voltar): ");
            string e = Console.ReadLine()?.Trim() ?? "";
            if (e == "0") return;
            aluno = BuscarAluno(e);
            if (aluno == null) Console.WriteLine("Aluno não encontrado. Tente novamente.");
        }

        Disciplina? disc = null;
        while (disc == null)
        {
            Console.Write("Nome ou código da disciplina (0 para voltar): ");
            string e = Console.ReadLine()?.Trim() ?? "";
            if (e == "0") return;
            disc = BuscarDisciplina(e);
            if (disc == null) Console.WriteLine("Disciplina não encontrada. Tente novamente.");
        }

        bool jaExiste = matriculas.Any(m => m.MatriculaAluno == aluno.Matricula && m.CodDisciplina == disc.Codigo);
        if (jaExiste)
        {
            Console.WriteLine($"\nAluno '{aluno.Nome}' já está matriculado em '{disc.Nome}'!");
        }
        else
        {
            matriculas.Add(new Matricula { CodDisciplina = disc.Codigo, MatriculaAluno = aluno.Matricula });
            Console.WriteLine($"\nMatrícula realizada com sucesso! {aluno.Nome} → {disc.Nome}");
        }
        Pausar();
    }

    static void AtribuirNota()
    {
        LimparTela();
        Console.WriteLine("=== Atribuir Nota ao Aluno ===\n");

        Aluno? aluno = null;
        while (aluno == null)
        {
            Console.Write("Nome ou matrícula do aluno (0 para voltar): ");
            string e = Console.ReadLine()?.Trim() ?? "";
            if (e == "0") return;
            aluno = BuscarAluno(e);
            if (aluno == null) Console.WriteLine("Aluno não encontrado. Tente novamente.");
        }

        Disciplina? disc = null;
        while (disc == null)
        {
            Console.Write("Nome ou código da disciplina (0 para voltar): ");
            string e = Console.ReadLine()?.Trim() ?? "";
            if (e == "0") return;
            disc = BuscarDisciplina(e);
            if (disc == null) Console.WriteLine("Disciplina não encontrada. Tente novamente.");
        }

        var mat = matriculas.FirstOrDefault(m => m.MatriculaAluno == aluno.Matricula && m.CodDisciplina == disc.Codigo);
        if (mat == null)
        {
            Console.WriteLine($"\nAluno '{aluno.Nome}' não está matriculado em '{disc.Nome}'!");
            Pausar();
            return;
        }

        Console.Write("Nota 1: ");
        if (!TryParseDouble(Console.ReadLine(), out double n1)) { Console.WriteLine("Nota inválida!"); Pausar(); return; }

        Console.Write("Nota 2: ");
        if (!TryParseDouble(Console.ReadLine(), out double n2)) { Console.WriteLine("Nota inválida!"); Pausar(); return; }

        mat.Nota1 = n1;
        mat.Nota2 = n2;
        double media = (n1 + n2) / 2.0;
        string sit = media >= disc.NotaMinima ? "Aprovado" : "Reprovado";
        Console.WriteLine($"\nNotas atribuídas! Média: {media:F1} → {sit}");
        Pausar();
    }

    // AUXILIARES

    static Aluno? BuscarAluno(string entrada)
    {
        if (int.TryParse(entrada, out int mat))
            return alunos.FirstOrDefault(a => a.Matricula == mat);
        return alunos.FirstOrDefault(a => a.Nome.Equals(entrada, StringComparison.OrdinalIgnoreCase));
    }

    static Disciplina? BuscarDisciplina(string entrada)
    {
        if (int.TryParse(entrada, out int cod))
            return disciplinas.FirstOrDefault(d => d.Codigo == cod);
        return disciplinas.FirstOrDefault(d => d.Nome.Equals(entrada, StringComparison.OrdinalIgnoreCase));
    }

    // Aceita vírgula ou ponto como separador decimal
    static bool TryParseDouble(string? s, out double value)
    {
        s = s?.Trim().Replace(',', '.');
        return double.TryParse(s, NumberStyles.Any, Invariant, out value);
    }

    static void Pausar()
    {
        Console.Write("\nPressione qualquer tecla para continuar...");
        if (Console.IsInputRedirected)
            Console.ReadLine();
        else
            Console.ReadKey(true);
    }

    static void LimparTela()
    {
        if (!Console.IsInputRedirected)
            Console.Clear();
        else
            Console.WriteLine();
    }
}
