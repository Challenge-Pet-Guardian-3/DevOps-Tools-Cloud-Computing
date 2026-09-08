# ============================================================
# Stage 1 — BUILD: compila o projeto com Gradle
# ============================================================
FROM gradle:jdk17 AS build

WORKDIR /app

# Copia apenas os arquivos de dependências primeiro (melhora cache de layers)
COPY build.gradle settings.gradle ./
COPY gradle/ gradle/
RUN gradle dependencies --no-daemon || true

# Copia o restante do código-fonte e builda
COPY src/ src/
RUN gradle bootJar --no-daemon -x test

# ============================================================
# Stage 2 — RUNTIME: imagem mínima de produção
# ============================================================
FROM eclipse-temurin:17-jre-alpine AS runtime

WORKDIR /app

# Cria usuário e grupo sem privilégios administrativos (non-root)
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

# Copia o JAR gerado no estágio de build
COPY --from=build /app/build/libs/pet-guardian-0.0.1-SNAPSHOT.jar app.jar

# Copia os recursos de chaves RSA para JWT
COPY --from=build /app/src/main/resources/keys/ /app/keys/

# Define permissões corretas para o usuário não privilegiado
RUN chown -R appuser:appgroup /app

# Executa como usuário sem privilégios administrativos
USER appuser

EXPOSE 8091

ENTRYPOINT ["java", "-jar", "app.jar"]
