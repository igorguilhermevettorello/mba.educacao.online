# Fluxo de Pagamento e Matrícula

## Arquitetura Ajustada

### Fluxo Anterior (Problemático)
```
ProcessarPagamentoPedidoCommandHandler
    ↓
MatriculaConfirmadaEvent (disparado ANTES do pagamento) ❌
    ↓
PagamentoEventHandler → PagamentoService (assíncrono)
    ↓
PagamentoRealizadoEvent (após pagamento bem-sucedido)
    ↓
PagamentoRealizadoEventHandler (cria matrículas)
```

**Problemas:** 
1. O evento `MatriculaConfirmadaEvent` era disparado ANTES do pagamento
2. O handler não aguardava o resultado do pagamento
3. Fluxo completamente assíncrono sem controle de resultado

---

### Fluxo Atual (Correto - Orquestrado)
```
ProcessarPagamentoPedidoCommandHandler
    ↓
PagamentoService.RealizarPagamentoPedido() (chamada SÍNCRONA) ⏳
    ↓
Aguarda resultado da transação...
    ↓
Se APROVADO ✅:
    ├─→ PagamentoRealizadoEvent → PagamentoRealizadoEventHandler (atualiza status do pedido)
    └─→ MatriculaConfirmadaEvent → MatriculaConfirmadaEventHandler (cria matrículas)
    
Se RECUSADO ❌:
    └─→ Retorna erro ao handler → Notifica usuário
```

---

## Componentes e Responsabilidades

### 1. **ProcessarPagamentoPedidoCommandHandler**
- **Função:** Orquestrador principal do fluxo de pagamento
- **Responsabilidades:**
  - Valida o pedido
  - Chama **sincronamente** o `PagamentoService`
  - Aguarda o resultado da transação
  - Se aprovado: dispara `MatriculaConfirmadaEvent`
  - Se recusado: retorna erro para o controller

### 2. **PagamentoService**
- **Função:** Processa o pagamento via gateway
- **Responsabilidades:**
  - Cria entidades de Pagamento e Transação
  - Chama o gateway de pagamento
  - Persiste dados no banco
  - Dispara `PagamentoRealizadoEvent` se bem-sucedido
  - Retorna objeto `Transacao` com o status

### 3. **PagamentoRealizadoEvent**
- **Quando:** Disparado pelo `PagamentoService` após pagamento aprovado
- **Objetivo:** Atualizar o status do pedido para "Pago"
- **Handler:** `PagamentoRealizadoEventHandler`

### 4. **MatriculaConfirmadaEvent**
- **Quando:** Disparado pelo handler APÓS pagamento ser aprovado
- **Objetivo:** Criar as matrículas do aluno nos cursos
- **Handler:** `MatriculaConfirmadaEventHandler`

---

## Arquivos Criados/Modificados/Removidos

### Novos Arquivos ✨
1. `MatriculaConfirmadaEventHandler.cs` - Cria matrículas após pagamento aprovado

### Arquivos Modificados 🔧
1. `ProcessarPagamentoPedidoCommandHandler.cs` - Chama **sincronamente** o `PagamentoService` e aguarda resultado
2. `RealizarMatriculaComPagamentoCommandHandler.cs` - Mesma abordagem síncrona
3. `PagamentoService.cs` - Removido disparo do `MatriculaConfirmadaEvent` (responsabilidade do handler)
4. `PagamentoRealizadoEventHandler.cs` - Simplificado para apenas atualizar status do pedido
5. `PagamentoPedido.cs` - Adicionado campo `ListaCursos`
6. `IPedidoRepository.cs` - Adicionado método `ObterPorId`
7. `MBA.Educacao.Online.Vendas.Application.csproj` - Adicionada referência a `Pagamentos.Domain`

### Arquivos Removidos 🗑️
1. `PagamentoEventHandler.cs` - Não é mais necessário (chamada direta ao serviço)
2. `PedidoIniciadoEvent.cs` - Não é mais necessário após refatoração para chamada síncrona

---

## Benefícios da Nova Arquitetura

✅ **Controle de Fluxo:** Handler aguarda o resultado do pagamento antes de prosseguir  
✅ **Semântica Correta:** `MatriculaConfirmadaEvent` disparado APÓS pagamento aprovado  
✅ **Tratamento de Erro:** Pagamentos recusados são tratados imediatamente  
✅ **Separação de Responsabilidades:** Handler orquestra, serviço processa, eventos notificam  
✅ **Transações Consistentes:** Operações síncronas garantem integridade dos dados  
✅ **Rastreabilidade:** Fluxo claro e fácil de debugar  
✅ **UX Melhorada:** Usuário recebe feedback imediato sobre o pagamento

## Diagrama de Sequência

```
Controller
    ↓ [envia Command]
ProcessarPagamentoPedidoCommandHandler
    ↓ [chama sync]
PagamentoService
    ↓ [processa]
Gateway de Pagamento
    ↓ [retorna]
PagamentoService
    ↓ [salva BD + dispara evento]
PagamentoRealizadoEventHandler ← [atualiza pedido]
    ↓ [retorna Transacao]
ProcessarPagamentoPedidoCommandHandler
    ↓ [verifica transacao.FoiPago()]
    ├─ SE APROVADO → dispara MatriculaConfirmadaEvent
    │   ↓
    │   MatriculaConfirmadaEventHandler ← [cria matrículas]
    │   ↓
    │   [retorna true ao Controller]
    │
    └─ SE RECUSADO → retorna false + notificação de erro
```  

