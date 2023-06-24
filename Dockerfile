# Imagem base
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Define o diretório de trabalho
WORKDIR /app

# Copia o arquivo de projeto e restaura as dependências
COPY src/Sln.Estoque.Application.Service/*.csproj ./src/Sln.Estoque.Application.Service/
COPY src/Sln.Estoque.Domain/*.csproj ./src/Sln.Estoque.Domain/
COPY src/Sln.Estoque.Infra.Data/*.csproj ./src/Sln.Estoque.Infra.Data/
COPY src/Sln.Estoque.Web/*.csproj ./src/Sln.Estoque.Web/
COPY Sln.Estoque.sln .
RUN dotnet restore

# Copia os arquivos do projeto e publica o aplicativo
COPY src/ ./src/
RUN dotnet publish Sln.Estoque.sln -c Release -o out

# Imagem final
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Define o diretório de trabalho
WORKDIR /app

# Copia os arquivos publicados para o container
COPY --from=build /app/out ./

# Define a variável de conexão
ENV CONNECTION_STRING="Server=192.168.80.106,9015;Database=Estoque;User Id=sa;Password=AdminMaster2023!;TrustServerCertificate=True;"

# Define a variável de TimeZone
ENV TZ=America/Sao_Paulo

# Expõe a porta 80
EXPOSE 80

# Define o comando de inicialização do container
ENTRYPOINT ["dotnet", "Sln.Estoque.Web.dll"]
