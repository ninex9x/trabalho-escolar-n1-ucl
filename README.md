# AED II - Trabalho N1

Programa de console em C# para gerenciamento e cálculo de resultados de provas de alunos.

## Descrição

O sistema permite cadastrar alunos, disciplinas e matrículas, atribuir notas e consultar o desempenho dos alunos. Todas as informações são persistidas em arquivos de texto (`.dat`) e carregadas automaticamente ao iniciar o programa.

## Estrutura dos Arquivos de Dados

Os arquivos são criados na pasta do executável com campos separados por ponto e vírgula (`;`):

- `Alunos.dat` — `Matricula;Nome;Idade`
- `Disciplinas.dat` — `Codigo;Nome;NotaMinima`
- `Matriculas.dat` — `CodDisciplina;MatriculaAluno;Nota1;Nota2`

## Funcionalidades

### Consultas

- **Alunos** — lista todos os alunos com matrícula, nome e idade.
- **Disciplinas** — lista todas as disciplinas com código, nome e nota mínima.
- **Alunos da Disciplina** — exibe todos os alunos de uma disciplina com suas notas, média e situação (Aprovado/Reprovado). Aceita nome ou código da disciplina.
- **Disciplinas do Aluno** — exibe todas as disciplinas de um aluno com suas notas, média e situação. Aceita nome ou número de matrícula.

### Cadastros

- **Alunos** — solicita nome e idade. O número de matrícula é gerado automaticamente (único).
- **Disciplinas** — solicita nome e nota mínima. O código é gerado automaticamente (único).
- **Matrículas** — vincula um aluno a uma disciplina. Aceita nome ou código para ambos.
- **Atribuir Nota** — informa Nota 1 e Nota 2 para um aluno em uma disciplina. Exibe a média e a situação ao salvar.

### Salvar

Grava manualmente os dados dos vetores nos arquivos `.dat`.

### Sair

Salva os dados automaticamente e encerra o programa.

## Regra de Aprovação

A média é calculada como `(Nota1 + Nota2) / 2`.

- **Aprovado:** média maior ou igual à nota mínima da disciplina.
- **Reprovado:** média menor que a nota mínima da disciplina.

## Requisitos

- .NET 8 SDK ou superior
- Visual Studio 2022 (recomendado) ou `dotnet CLI`

## Como Executar

**Visual Studio:** abra o arquivo `AED_II_N1.sln` e pressione `F5`.

**CLI:**
```bash
cd AED_II_N1
dotnet run
```
