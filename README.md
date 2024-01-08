![CodeFlow](CodeFlowUI/Resources/logo-nome.png)

<h3> *Este é um projeto desenvolvido como avaliação individual final do módulo Programação Orientada a Objetos I do curso de backend em C# da AdaTech em parceria com o Mercado Eletrônico. </h3>

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white)

<h3>CodeFlow é um aplicativo desktop para gerenciamento de equipes de desenvolvimento de software, onde um TechLeader coordena diversas equipes :)</h3>

## Table of Contents

- [Como usar](#como-usar)
- [Documentação](#documentação)
- [Autenticação](#autenticação)
- [Database](#database)
- [Estrutura do projeto](#estrutura-do-projeto)
- [Funcionalidades](#funcionalidades)

## Como usar

- Baixe e instale a fonte Ubuntu do Google Fonts.
- Clone o repositório e defina o projeto UI como startup project.
Agora o projeto está pronto para ser executado.

## Documentação



## Autenticação

Este projeto possui dois tipos de usuários: TechLeader e Developer. Ambos podem criar suas próprias contas e logo após utilizar a aplicação normalmente.

Apenas o TechLeader pode criar um projeto e para um developer entrar no projeto é preciso que ele seja convidado e que ele entre no projeto com o código do projeto.

## Database
Este projeto utiliza [SQLite]([https://www.sqlite.org/index.html) como banco de dados. 


## Estrutura do projeto

A solution foi dividida em duas partes: UI (a interface) e Backend (lógica de negócio).
Dentro do backend, o projeto foi divido entre services e repositories. UI faz solicitações com DTOs (se necessário) para o Service correspondente e o Service chama um (ou vários) métodos do Repository correspondente (que pode devolver uma Entity). 

## Funcionalidades

<h3>TechLeader</h3>
- Criar projeto
- Criar Task
- Atualizar os detalhes de um projeto, como sua data de entrega
- Atribuir Task a algum developer
- Convidar Developer para um projeto
- Remover um developer de um projeto
- Alterar uma Task
- Definir uma data para uma Task criada por Developer e consequentemente liberar ela para desenvolvimento
- Visualizar todas as Tasks de um projeto
- Criar TAG
- Definir uma TAG como done
- Ver a quantidade de tasks em cada status
- Ver a quantidade de projetos em cada status
- Atualizar as suas informações 

<h3>Developer</h3>
- Entrar em um projeto na qual foi convidado
- Criar Task (porém ela será atribuída a o próprio developer e só ficará disponível para modificações de progresso após o TechLeader do projeto definir uma data)
- Criar TAG
- Ao completar toda a checklist de uma Task, enviar para review do tech leader
- Visualizar todas as suas próprias tasks do projeto e as Tasks que tem as mesmas tags que as suas tasks
- Atualizar suas informações 

