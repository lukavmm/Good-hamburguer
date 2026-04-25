# ?? GoodHamburger - Frontend Blazor WebAssembly

Frontend em **Blazor WebAssembly** consumindo a API REST do GoodHamburger.

## ?? Como Executar

### Pré-requisitos
1. Certifique-se de que a **API backend** está rodando em `https://localhost:5001`

### Executar o Frontend

```bash
cd frontend/GoodHamburger.BlazorApp
dotnet run
```

Acesse: **https://localhost:5002** (ou a porta indicada no terminal)

## ? Funcionalidades

### Página Inicial (Home)
- ? Visualizar cardápio completo
- ? Selecionar itens (1 sanduíche obrigatório + extras opcionais)
- ? Ver resumo do pedido em tempo real
- ? Cálculo automático de desconto
- ? Finalizar pedido

### Página de Pedidos
- ? Listar todos os pedidos realizados
- ? Ver detalhes de cada pedido
- ? Cancelar pedidos

## ?? Interface

- Design moderno com gradiente roxo
- Cards interativos para seleção de itens
- Feedback visual para itens selecionados
- Indicadores de tipo de item (Sanduíche/Fritas/Bebida)
- Cálculo de desconto em tempo real
- Mensagens de sucesso/erro

## ??? Tecnologias

- Blazor WebAssembly (.NET 8)
- HttpClient para consumo da API
- CSS personalizado
- Dependency Injection

## ?? Endpoints Consumidos

- `GET /api/menu` - Buscar cardápio
- `POST /api/orders` - Criar pedido
- `GET /api/orders` - Listar pedidos
- `DELETE /api/orders/{id}` - Cancelar pedido
