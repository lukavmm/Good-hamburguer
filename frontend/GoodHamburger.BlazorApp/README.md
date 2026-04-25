# ?? GoodHamburger - Sistema de Pedidos

Sistema de gerenciamento de pedidos de hamburgueria desenvolvido em **Blazor WebAssembly** com autenticação JWT e autorização baseada em roles.

## ?? Tecnologias Utilizadas

- **Frontend**: Blazor WebAssembly (.NET 8.0)
- **Autenticação**: JWT (JSON Web Tokens)
- **Autorização**: Role-based (Admin/Normal)
- **Persistência**: LocalStorage (via IJSRuntime)
- **Estilização**: CSS3 com gradientes e animações
- **Ícones**: Bootstrap Icons v1.11.3
- **HTTP Client**: IHttpClientFactory com DelegatingHandler pattern

## ?? Funcionalidades

### Usuário Normal
- ? Login e registro de conta
- ? Visualizar cardápio com hambúrgueres, batatas e bebidas
- ? Criar pedidos com seleção de itens
- ? Sistema de descontos automáticos (combos)
- ? Visualizar histórico de pedidos
- ? Editar pedidos existentes
- ? Cancelar pedidos

### Administrador
- ? Todas as funcionalidades de usuário normal
- ? Gerenciar cardápio (CRUD completo)
- ? Criar, editar e remover itens do menu
- ? Definir preços e categorias

## ??? Arquitetura

### Padrões Implementados

**Service Layer Pattern**
- Separação clara entre lógica de negócio e apresentação
- Serviços injetados via Dependency Injection

**Repository Pattern** (via Services)
- `IAuthService` - Gerenciamento de autenticação
- `IMenuService` - Acesso ao cardápio
- `IOrderService` - Gerenciamento de pedidos
- `IMenuItemManagementService` - CRUD de itens (Admin)

**Authorization Pattern**
- `SimpleAuthStateProvider` - Provedor de estado de autenticação
- `SimpleAuthorizationMessageHandler` - Intercepta requests HTTP para adicionar Bearer token
- `CascadingAuthenticationState` - Propaga estado de autenticação pela árvore de componentes

**Dependency Injection Lifetime Management**
- **Singleton**: Serviços de autenticação e estado global (AuthServiceSimple, SimpleAuthStateProvider, ToastService)
- **Scoped**: Serviços de dados por requisição (MenuService, OrderService)
- **IHttpClientFactory**: Para gerenciar HttpClient sem conflitos de lifetime

### Named HttpClients

**AuthClient** (Não autenticado)
- Base URL: https://localhost:60059
- Usado para: Login e Registro
- Sem interceptação de token

**ApiClient** (Autenticado)
- Base URL: https://localhost:60059
- Usado para: Todas as chamadas que requerem autenticação
- Interceptado por `SimpleAuthorizationMessageHandler`
- Adiciona automaticamente `Authorization: Bearer {token}`

## ?? Autenticação e Segurança

### Fluxo de Autenticação

1. **Login**: Usuário envia credenciais ? API retorna JWT token
2. **Persistência**: Token armazenado no LocalStorage do navegador
3. **Estado**: `AuthenticationStateProvider` atualizado com claims do usuário
4. **Autorização**: Componentes protegidos com `[Authorize]` ou `[Authorize(Roles = "Admin")]`
5. **HTTP Requests**: `SimpleAuthorizationMessageHandler` adiciona token automaticamente

### Inicialização

```csharp
// Program.cs - Carrega autenticação do LocalStorage ao iniciar
var authService = app.Services.GetRequiredService<IAuthService>();
if (authService is AuthServiceSimple authServiceSimple)
{
    await authServiceSimple.InitializeAsync();
}
```

### LocalStorage Keys
- `authToken` - JWT token
- `authUsername` - Nome do usuário
- `authRole` - Role do usuário (Admin/Normal)

## ?? UI/UX

### Design System
- **Gradientes**: `#667eea` ? `#764ba2` (Tema principal)
- **Cores secundárias**: 
  - Sucesso: `#51cf66` ? `#37b24d`
  - Erro: `#ff6b6b` ? `#ee5a6f`
  - Info: `#48dbfb` ? `#0abde3`
- **Border Radius**: 12-16px (consistente)
- **Sombras**: Múltiplas camadas para profundidade
- **Animações**: Transições suaves (0.3s ease)

### Componentes

**Toast Notifications**
- Sistema de notificações não-intrusivo
- Auto-dismiss após 5 segundos
- Tipos: Success, Error, Info, Warning
- Posição: Top-right (responsivo para mobile)

**Modais**
- Background com blur effect
- Animação slide-up
- Responsivo (95% width em mobile)
- Cabeçalho e rodapé consistentes

**Cards**
- Menu items com hover effects
- Seleção visual com border highlight
- Badges coloridos por categoria
- Shadow elevation no hover

## ?? Responsividade

### Breakpoints

**Desktop** (> 768px)
- Grid de 3-4 colunas para menu
- Navegação horizontal
- Modais centralizados (max-width: 500px)

**Mobile** (? 768px)
- Grid de 1 coluna
- Navegação vertical
- Botão logout full-width
- Modais full-screen (95% width)
- Toast adaptado para toda largura

## ??? Estrutura de Arquivos

```
GoodHamburger.BlazorApp/
??? Models/
?   ??? AuthResponse.cs
?   ??? LoginRequest.cs
?   ??? RegisterRequest.cs
?   ??? MenuItemModel.cs
?   ??? OrderModel.cs
?   ??? CreateOrderRequest.cs
?   ??? UpdateOrderRequest.cs
??? Services/
?   ??? IAuthService.cs
?   ??? AuthServiceSimple.cs
?   ??? SimpleAuthStateProvider.cs
?   ??? SimpleAuthorizationMessageHandler.cs
?   ??? LocalStorageService.cs
?   ??? IMenuService.cs
?   ??? MenuService.cs
?   ??? IOrderService.cs
?   ??? OrderService.cs
?   ??? IMenuItemManagementService.cs
?   ??? MenuItemManagementService.cs
?   ??? ToastService.cs
??? Pages/
?   ??? Login.razor
?   ??? Register.razor
?   ??? Home.razor
?   ??? Orders.razor
?   ??? MenuManagement.razor
??? Shared/
?   ??? MainLayout.razor
?   ??? AuthLayout.razor
?   ??? NavMenu.razor
?   ??? SimpleNotAuthorized.razor
?   ??? ToastContainer.razor
??? wwwroot/
?   ??? css/
?       ??? app.css
??? App.razor
??? Program.cs
```

## ?? Configuração

### API Backend

Certifique-se de que sua API está rodando em:
```
https://localhost:60059
```

### Endpoints Esperados

**Autenticação**
- `POST /api/Auth/login` - Login
- `POST /api/Auth/register` - Registro

**Menu**
- `GET /api/menu` - Listar menu

**Pedidos**
- `GET /api/Orders` - Listar pedidos
- `POST /api/Orders` - Criar pedido
- `PUT /api/Orders/{id}` - Atualizar pedido
- `DELETE /api/Orders/{id}` - Deletar pedido

**Gerenciamento (Admin)**
- `GET /api/MenuItems` - Listar itens
- `POST /api/MenuItems` - Criar item
- `PUT /api/MenuItems/{id}` - Atualizar item
- `DELETE /api/MenuItems/{id}` - Deletar item

## ?? Como Executar

1. Clone o repositório
2. Certifique-se de que a API está rodando em `https://localhost:60059`
3. Execute o projeto:
```bash
dotnet run
```
4. Acesse: `https://localhost:7001`

## ?? Roles de Usuário

### Enum da API
```csharp
public enum UserRole
{
    Admin = 0,
    Normal = 1
}
```

### Criação de Usuários

**Administrador** (Role = 0)
- Acesso total ao sistema
- Pode gerenciar cardápio

**Usuário Normal** (Role = 1)
- Pode fazer e gerenciar pedidos
- Visualiza apenas o cardápio

## ?? Decisões Técnicas

### Por que Singleton para Autenticação?

A autenticação foi implementada como **Singleton** para:
- Manter estado consistente durante toda a sessão
- Evitar perda de token ao navegar entre páginas
- Garantir que todos os componentes acessem o mesmo estado
- Sobreviver ao ciclo de vida do Scoped (que é recriado a cada navegação no Blazor WASM)

### Por que IHttpClientFactory?

- Evita conflito de Dependency Injection Lifetime (Singleton ? Scoped)
- Permite configurações diferentes (AuthClient vs ApiClient)
- Reutilização eficiente de HttpClient
- Melhor gerenciamento de conexões

### Por que LocalStorage em vez de SessionStorage?

- Persistência além do fechamento do navegador (opcional)
- Melhor experiência do usuário (não precisa fazer login sempre)
- Fácil de limpar no logout

### Por que não usar Blazored.LocalStorage?

Durante o desenvolvimento, a biblioteca `Blazored.LocalStorage` causou deadlocks de inicialização assíncrona, bloqueando o render da aplicação. A implementação customizada com `IJSRuntime` provou ser mais confiável e sem dependências externas.

## ?? Problemas Conhecidos Resolvidos

1. **CascadingAuthenticationState Blocking**: Resolvido com GetAuthenticationStateAsync síncrono
2. **Circular Dependency**: Resolvido com IServiceProvider para resolver AuthenticationStateProvider
3. **Token Loss on Navigation**: Resolvido com Singleton lifetime
4. **Blazored.LocalStorage Deadlock**: Substituído por implementação customizada

## ?? Notas para o Entrevistador

Este projeto demonstra:
- ? Conhecimento sólido de Blazor WebAssembly
- ? Implementação completa de autenticação JWT
- ? Autorização baseada em roles
- ? Padrões de design (Service Layer, DI, DelegatingHandler)
- ? Gerenciamento cuidadoso de lifetime de DI
- ? UI/UX responsivo e moderno
- ? Código limpo e organizado
- ? Tratamento de erros com feedback ao usuário
- ? Persistência de estado com LocalStorage

## ?? Licença

Este projeto foi desenvolvido como demonstração técnica.

---

Desenvolvido com ?? usando Blazor WebAssembly
