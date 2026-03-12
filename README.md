# Aula 3D - Visualizador e Interação de Objetos 3D com Visão Computacional

Este projeto é um visualizador de objetos 3D desenvolvido na Godot Engine utilizando C#. Ele permite carregar modelos 3D em tempo de execução, manipulá-los (escala, translação, rotação) e aplicar shaders de corte (clipping) através de uma interface de usuário simples ou através de **Visão Computacional (Rastreamento de Mãos)**.

## Arquitetura Geral do Projeto

A solução é dividida em três frentes principais:
- **Aula3D.App**: O projeto principal Godot que gerencia o visualizador 3D, UI, instanciamento de modelos e a aplicação das transformações baseadas nos inputs (mouse ou visão).
- **Aula3D.VisionCore**: A biblioteca central em .NET responsável pelo pipeline de Visão Computacional (processamento de imagem e extração da pose/gesto da mão usando OpenCV).
- **Aula3D.VisionConsole**: Uma aplicação de console isolada para debugar, testar e calibrar as máscaras HSV e a lógica de processamento de imagem sem precisar da Godot Engine rodando.

*Consulte o arquivo [Instructions.md](Instructions.md) para ler documentações detalhadas sobre o funcionamento da arquitetura, matemática dos quaternions, e a descrição linha-a-linha dos pipelines.*

## Funcionalidades
- **Carregamento Dinâmico de Modelos:** Suporte em runtime para arquivos `.gltf` e `.glb`.
- **Manipulação por Visão ou Interface:** Rotacione, posicione e escale modelos utilizando os controles de UI na tela ou faça isso movendo sua mão em frente à webcam (exige OpenCV suportado no ambiente).
- **Matemática Avançada de Rotação:** O projeto utiliza Quaternions para rotações livres contínuas e seguras contra o fenômeno Gimbal Lock.
- **Shader de Corte (Clipping):** Remova seções do modelo 3D em diferentes eixos diretamente pelo slider de corte de shader.
- **Integração Desacoplada:** O repositório utiliza Injeção de Dependências/Padrão Facade. O projeto de Visão roda numa thread separada provendo dados passivos `(X, Y, Z, HandState)` para o loop de física da Godot através da interface `IGestureProvider`.
- **Mouse Mock:** Um adaptador "mouse fake" (`MouseMock.cs`) caso deseje desenvolver sem uma webcam.

## Pré-requisitos
Para compilar e rodar este projeto, você precisará de:
1. **Godot Engine:** Versão da Godot 4.x **com suporte ao .NET (Mono C#)**.
2. **.NET SDK:** Versão 8.0 ou 10.0 para compilar a solution (VisionCore foi construído em .NET 10.0).
3. **OpenCV (.NET EmguCV ou OpenCvSharp):** As dependências NuGet do OpenCV são resolvidas no build, certifique-se de estar num SO suportado (Windows/Linux/MacOS) com ambiente capaz de processar gráficos.

## Como Fazer Build e Rodar

### Rodando o Visualizador Godot (Aula3D.App)

1. Abra o **Godot Engine (versão .NET)**.
2. Clique em `Importar` e selecione o arquivo `project.godot` na pasta `Aula3D.App/`.
3. Com o projeto aberto na Godot, clique no ícone **Build** (ícone de martelo no canto superior direito) para restaurar os pacotes NuGet da Core e compilar os scripts.
4. Clique no ícone de Play (ou pressione `F5`).

**Executando por linha de comando:**
```bash
cd Aula3D.App
dotnet build
godot-mono --path .
```

### Rodando a Ferramenta de Teste de Visão (Aula3D.VisionConsole)

Útil para debugar se a sua câmera está sendo reconhecida e ver em tempo real as máscaras geradas:

```bash
cd Aula3D.VisionConsole
dotnet run
```

## Estrutura de Pastas

```text
/Aula3D
├── Aula3D.App/                  # Projeto C# Godot (Modelos 3D, Interface Física e Gráficos)
│   ├── project.godot            # Definições do projeto Godot
│   ├── Cenas/                   # Cenas tscn (Ex: Main.tscn)
│   ├── Scripts/     
│   │   ├── Adapters/            # Implementações fake de tracking (Ex: MouseMock.cs)
│   │   ├── Controladores/       # GameManagers e manipuladores (Ex: Objeto3D.cs, CameraController.cs)
│   │   └── Utilitarios/         # Conversores
│   └── Shaders/                 # Shaders avançados de Clipping GLSL
├── Aula3D.VisionCore/           # Core de Processamento OpenCV (Decoupled System)
│   ├── GestorDeVisaoFacade.cs   # Facade de integração Singleton com Threads
│   ├── Processamento/           # Classificadores (Filtros, Defectos Convexos e Momentos Hu)
│   └── Interfaces/              # Contratos consumidos pelo App Godot (IGestureProvider)
└── Aula3D.VisionConsole/        # Application de console CLI apenas para diagnostico da Câmera
    └── Program.cs
```
