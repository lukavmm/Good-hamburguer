# 🍔 Good Hamburger API

API de gerenciamento de pedidos para uma hamburgueria, desenvolvida com foco em:

- **Clean Architecture**
- **Princípios SOLID**
- **TDD (Test-Driven Development)**

---

## 🚀 Visão Geral

Este projeto demonstra como construir uma API escalável, testável e de fácil manutenção utilizando boas práticas modernas no ecossistema .NET.

### ✨ Destaques

- Arquitetura desacoplada em camadas
- Regras de negócio isoladas no domínio
- Autenticação com JWT e controle por roles
- Cobertura de testes nos principais fluxos
- Código organizado e preparado para evolução

---

## 🧱 Arquitetura

```
GoodHamburger/
├── src/
│   ├── GoodHamburger.Domain/         # Regras de negócio
│   ├── GoodHamburger.Application/    # Casos de uso
│   ├── GoodHamburger.Infrastructure/ # Persistência e serviços
│   └── GoodHamburger.API/            # Camada de entrada (HTTP)
└── tests/
```

### 🔍 Camadas

| Camada | Responsabilidade |
|--------|-----------------|
| **Domain** | Entidades, regras de negócio, interfaces |
| **Application** | Casos de uso, DTOs, validações |
| **Infrastructure** | Banco de dados, repositórios, serviços |
| **API** | Controllers, middlewares, configuração |

---

## 🛠️ Tecnologias

- **.NET 8** / C# 12
- **ASP.NET Core Web API**
- **Entity Framework Core** (In-Memory)
- **JWT Bearer Authentication**
- **BCrypt.Net**
- **xUnit** + **FluentAssertions** + **NSubstitute**
- **Swagger / OpenAPI**

---

## ⚙️ Como Executar

### 📋 Pré-requisitos

- .NET 8 SDK instalado

### 🚀 Passos

1. **Clone o repositório:**
```bash
git clone <url-do-repositorio>
cd GoodHamburger
```

2. **Restaure as dependências:**
```bash
dotnet restore
```

3. **Execute a aplicação:**
```bash
cd src/GoodHamburger.API
dotnet run
```

4. **Acesse o Swagger:**
```
https://localhost:<porta>/swagger
```

---

## 🔐 Autenticação

A API utiliza **JWT Bearer Token** com controle de acesso baseado em roles.

### 👤 Perfis

| Perfil | Permissões |
|--------|-----------|
| **Admin** | Gerencia menu e pedidos |
| **User** | Gerencia apenas pedidos |

### 🔑 Usuário Padrão

```json
{
  "username": "admin",
  "password": "admin"
}
```

### 📌 Fluxo de Autenticação

1. **Login em:**
   ```
   POST /api/auth/login
   ```

2. **Receba o token**

3. **Envie no header:**
   ```
   Authorization: Bearer {token}
   ```

---

## 📡 Endpoints

### 🔐 Auth

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/auth/login` | Login |
| POST | `/api/auth/register` | Cadastro |

### 🍔 Menu

| Método | Rota | Acesso |
|--------|------|--------|
| GET | `/api/menu` | Público |
| GET | `/api/menuitems/{id}` | Público |
| POST | `/api/menuitems` | Admin |
| PUT | `/api/menuitems/{id}` | Admin |
| DELETE | `/api/menuitems/{id}` | Admin |

### 🧾 Pedidos

| Método | Rota | Acesso |
|--------|------|--------|
| GET | `/api/orders` | Autenticado |
| GET | `/api/orders/{id}` | Autenticado |
| POST | `/api/orders` | Autenticado |
| PUT | `/api/orders/{id}` | Autenticado |
| DELETE | `/api/orders/{id}` | Autenticado |

---

## 📏 Regras de Negócio

### 🧾 Pedidos

- Deve conter **exatamente 1 sanduíche**
- Não permite **itens duplicados**
- **Descontos automáticos:**

| Combinação | Desconto |
|-----------|----------|
| Sanduíche + Batata + Bebida | 20% |
| Sanduíche + Batata | 15% |
| Sanduíche + Bebida | 10% |
| Apenas Sanduíche | 0% |

### 🍟 Menu

- Nome **obrigatório**
- Preço **maior que zero**
- **Tipos:**
  - `Sandwich`
  - `Fries`
  - `Drink`

### 🔐 Usuários

- Username **único**
- Senha **mínima de 4 caracteres**
- Token **expira em 8 horas**

---

## 🧪 Testes

✅ **46 testes unitários**  
✅ **100% passando**

### 📊 Cobertura

- Regras de negócio (Order)
- CRUD completo (Menu e Pedidos)
- Autenticação

### ▶️ Executar testes

```bash
dotnet test
```

---

## 🎯 Decisões de Arquitetura

### Clean Architecture
- Separação clara de responsabilidades
- Independência de frameworks
- Alta testabilidade

### TDD
- Segurança para refatorações
- Código guiado por comportamento
- Testes como documentação

### In-Memory Database
- Simples para demonstração
- Sem necessidade de configuração externa
- Execução rápida

### JWT
- Stateless
- Escalável
- Padrão de mercado

---

## 📦 DTOs

### Requests
- `LoginRequest`
- `RegisterRequest`
- `CreateMenuItemRequest`
- `UpdateMenuItemRequest`
- `CreateOrderRequest`
- `UpdateOrderRequest`

### Responses
- `AuthResponse`
- `MenuItemResponse`
- `OrderResponse`

---

## 📄 Licença

Este projeto está sob a licença **MIT**.