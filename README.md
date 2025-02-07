# 📌 Documentação da API - Desafio BackEnd Manipulae

## **1️⃣ Visão Geral da API**
A API **Desafio BackEnd Manipulae** permite **buscar, cadastrar, atualizar, excluir e filtrar vídeos** baseando-se na **YouTube Data API v3** e armazenando os dados em um banco **SQLite**.

A API também conta com **autenticação JWT** e um sistema de permissões, onde:
- **Admin** pode criar, excluir e gerenciar vídeos.
- **Usuários comuns** podem apenas visualizar os vídeos.


## **2️⃣ Tecnologias Utilizadas**
✅ **.NET 7** - Backend
✅ **Entity Framework Core** - ORM para SQLite
✅ **JWT** - Autenticação e segurança
✅ **Moq/xUnit** - Testes unitários
✅ **Swagger** - Documentação e testes interativos


## **3️⃣ Como Rodar a API?**

### **Passo 1: Clonar o Repositório**
```sh
 git clone https://github.com/seu-repositorio.git
 cd DesafioBackEndManipulae
```

### **Passo 2: Instalar Dependências**
```sh
dotnet restore
```

### **Passo 3: Executar as Migrações do Banco**
```sh
dotnet ef database update
```

### **Passo 4: Rodar a API**
```sh
dotnet run
```

Por padrão, a API estará disponível em:
```
http://localhost:5000/swagger 
OBS: Como não fiz configuração explícita, a API ficou disponível para mim aqui: `https://localhost:7202/swagger` por isso indico executar direto no Visual Studio.
```

---
## **4️⃣ Testando a API no Swagger**
O Swagger permite testar a API de forma interativa.

### **📌 Acessar o Swagger**

Executando diretamente pelo VisualStudio já irá exibir no Swagger.

Ou

1. Inicie a API com `dotnet run`
2. No navegador, acesse: `http://localhost:5000/swagger` 
OBS: Como não fiz configuração explícita, a API ficou disponível para mim aqui: `https://localhost:7202/swagger` por isso indico executar direto no Visual Studio.

Lá você encontrará **todos os endpoints** e poderá testá-los diretamente.

---
## **5️⃣ Passo a Passo: Testando Endpoints no Swagger**

### **1️⃣ Criar um Usuário**
**Endpoint:** `POST /api/user/register`
**Body:**
```json
{
  "username": "admin",
  "password": "123456",
  "role": "Admin"
}
```
**Retorno esperado:**
```json
"Usuário cadastrado com sucesso."
```

### **2️⃣ Fazer Login e Obter Token JWT**
**Endpoint:** `POST /api/user/login`
**Body:**
```json
{
  "username": "admin",
  "password": "123456"
}
```
**Retorno esperado:**
```json
{
  "token": "eyJhbGciOiJI..."
}
```

### ** 3 Buscar Vídeos no YouTube com Filtro que desejar porém do ano de 2022**
**Endpoint:** `GET /api/YouTube/buscar?query=FILTRO_DESEJADO`
**Header:**
```
Authorization: Bearer SEU_TOKEN_AQUI (possível que solicite nova autorização)
```

### ** 4 Salvandos na base os vídeos buscados no YouTube**
**Endpoint:** `POST api/YouTube/salvar?query=FILTRO_DESEJADO`
**Header:**
```
Authorization: Bearer SEU_TOKEN_AQUI (possível que solicite nova autorização)
```


### ** 5 Buscar Vídeos com Filtro**
**Endpoint:** `GET /api/video/filtrar?titulo=Manipulação`
**Header:**
```
Authorization: Bearer SEU_TOKEN_AQUI (possível que solicite nova autorização)
```

**Retorno esperado:**
```json
[
  {
    "id": 1,
    "titulo": "Vídeo Manipulação",
    "autor": "Canal X",
    "descricao": "Conteúdo sobre manipulação de medicamentos",
    "url": "https://youtube.com/...",
    "dataPublicacao": "2022-01-01T00:00:00"
  }
]
```

### ** 6 Adicionar um Novo Vídeo**
**Endpoint:** `POST /api/video/adicionar`
**Body:**
```json
{
  "titulo": "Novo Vídeo",
  "autor": "Canal X",
  "descricao": "Descrição do vídeo",
  "url": "https://youtube.com/...",
  "dataPublicacao": "2022-01-01T00:00:00"
}
```

### ** 7 Atualizar um Vídeo**
**Endpoint:** `PUT /api/video/{id}`
**Body:**
```json
{
  "id": 1,
  "titulo": "Novo Título",
  "descricao": "Nova descrição"
}
```

### ** 8 Excluir um Vídeo (Apenas Admin)**
**Endpoint:** `DELETE /api/video/{id}`

**Header:**
```
Authorization: Bearer SEU_TOKEN_AQUI (possível que solicite nova autorização)
```

**Retorno esperado:**
```
204 No Content
```

---
## **6️⃣ Considerações Finais**
Esta API foi desenvolvida utilizando **boas práticas** como:
- **Autenticação JWT** para segurança
- **SQLite + Entity Framework** para persistência de dados
- **Testes automatizados com xUnit + Moq**
- **Tratamento de erros via Middleware**


