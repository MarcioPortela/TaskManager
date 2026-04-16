# TaskManager - API para controle de tarefas

![.NET Core](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Clean Architecture](https://img.shields.io/badge/Arquitetura-Clean-blue?style=for-the-badge)

Uma API REST construída em **.NET 8** para receber e controlar tarefas pendentes, em progresso e concluidas.

## 🛠️ Tecnologias Utilizadas

* **.NET 8.0**
* **Entity Framework Core (In-Memory)**
* **FluentValidation**
* **xUnit & FluentAssertions**
* **Swagger (OpenAPI)**

## 🏗️ Arquitetura e Padrões de Projeto

Esta aplicação foi construída seguindo a **Clean Architecture**. O projeto foi estritamente dividido em 4 camadas horizontais, respeitando a Regra de Dependência (de fora para dentro):

* **Domain:** Contém as Entidades (encapsulamento de estado), Enums e Interfaces. Nenhuma dependência externa.
* **Application:** Contém os Serviços, DTOs e as regras de validação (Fail-Fast).
* **Infrastructure:** Contém o Entity Framework Core (In-Memory Database) e a implementação do Padrão Repository.
* **Api:** Contém os Controllers, Swagger e Middlewares (como o Tratamento Global de Exceções).

### 1. Validação Fail-Fast (Pipeline Behaviors)
A validação de entrada é tratada usando **FluentValidation**. Requisições inválidas são interceptadas e rejeitadas *antes* mesmo de chegarem à regra de negócio, mantendo os Services limpos e focados apenas na execução da tarefa.

### 2. Tratamento Global de Exceções
Utilizando o `IExceptionHandler` do .NET 8, todas as exceções não tratadas e erros de validação são interceptados globalmente, evitando o vazamento de stack traces e garantindo respostas consistentes na API.

## ⚙️ Como Executar

1. Certifique-se de ter o [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
2. Clone o repositório e navegue até a pasta do projeto da API:
   ```bash
   cd TaskManager.Api
3. Execute a aplicação:
    ```bash
    dotnet run
4. Abra seu navegador e acesse o Swagger UI para testar os endpoints:
http://localhost:5000/swagger

## 📚 Endpoints da API

A API expõe as seguintes rotas baseadas na convenção REST:

**POST** `/api/v1/tasks`: Cria uma nova tarefa.

**GET** `/api/v1/tasks`: Lista todas as tarefas.

**PUT** `/api/v1/tasks/{id}`: Atualiza os dados de uma tarefa existente.

**DELETE** `/api/v1/tasks/{id}`: Remove uma tarefa do banco de dados.
