
Prova Hexagon
Foi desenvolvido o projeto com arquitetura MVC utilizando DotNet Core 6.0 para o backend e Angular para o FrontEnd.

#BackEnd
- MVC;
- Jwt;
- Migrations;
- EntityFrameWork 6;
- DotNet Core 6.0;
- Swagger;


#FrontEnd
- Dropdown Cascade de Estado e Cidade, é obtido a cidade através do estado selecionado com as tabelas sendo populadas pelo backend;
- Dropdown para estado civil com a tabela sendo populada pelo backend;
- Mascara de campos utilizando o ngx-mask-2;
- Mensagens de aviso utilizando o ngx-toastr;
- Paginação do Grid utilizando o ngx-pagination;
- Html em Bootstrap 5;
- Validação dos campos utilizando Validators do angular;
- Validação dos dados no backend sendo apresentados no frontend;

Tecnologia utilizada;
[Angular CLI](https://github.com/angular/angular-cli) version 14.0.1

#Executar o Projeto (BackEnd)
1 - Abrir o projeto WebApi.sln;
2 - Setar o nome do servidor de banco de dados no arquivo "appsettings.json" na conexão "DefaultConnection" (Este está rodando na minha máquina pelo - Sql Server Object Explorer);
3 - Rodar o Migration - No console - "add-migration ExameHexagon" em sequência "update-migration";
4 - Rodar o Projeto e abrir o swagger;
5 - Gerar usuário e senha chamando o POST "CreateLogin", deve passar um valor para "usuario" e "senha";
6 - Após o usuário criado, chamar o GET "login", passar usuário e senha criados e pegar o token gerado;
7 - Chamar o POST "CreateEstadoCivil" passando o nome - Ex.: "Casado" para criação dos dados da tabela de estado civil;
8 - Chamar o POST "CreateEstado" passando o nome - Ex.: "Rio de Janeiro" e a sigla - Ex. "RJ", para criação dos dados da tabela de estado;
9 - Chamar o POST "CreateCidade" passando o nome - Ex.: "Saquarema" e o estadoId obtido na criação do estado para criação dos dados da tabela de cidade;
10 - Executar a API;

#Executar o Projeto (FrontEnd)
11 - Executar o Frontend (ng serve);
12 - Autenticar no sistema com usuário e senha criados no passo 5;
