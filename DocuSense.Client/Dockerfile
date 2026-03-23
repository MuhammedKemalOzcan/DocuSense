# Temel Node.js imajı
FROM node:20-alpine AS base

# 1. AŞAMA: Bağımlılıkları Kurma (Dependencies)
FROM base AS deps
WORKDIR /app
# pnpm kullandığın için corepack'i aktif ediyoruz
RUN corepack enable pnpm
COPY package.json pnpm-lock.yaml* ./
RUN pnpm i --frozen-lockfile

# 2. AŞAMA: Derleme (Build)
FROM base AS builder
WORKDIR /app
RUN corepack enable pnpm
COPY --from=deps /app/node_modules ./node_modules
COPY . .
ARG NEXT_PUBLIC_API_URL
ENV NEXT_PUBLIC_API_URL=$NEXT_PUBLIC_API_URL
# Standalone modunda build alıyoruz
RUN pnpm build

# 3. AŞAMA: Çalıştırma (Runner - Sadece gerekli dosyalar)
FROM base AS runner
WORKDIR /app
ENV NODE_ENV=production
# Next.js'in gereksiz telemetri (veri toplama) isteklerini kapatıyoruz
ENV NEXT_TELEMETRY_DISABLED=1 

# Sadece Standalone klasöründeki çalışabilir dosyaları ve public varlıkları kopyalıyoruz
COPY --from=builder /app/public ./public
COPY --from=builder /app/.next/standalone ./
COPY --from=builder /app/.next/static ./.next/static

EXPOSE 3000
ENV PORT=3000

# Standalone modunun ürettiği optimize edilmiş sunucuyu başlatıyoruz
CMD ["node", "server.js"]