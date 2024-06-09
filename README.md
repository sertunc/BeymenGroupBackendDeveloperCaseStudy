# Configuration.Library

Bu proje, web.config, app.config gibi dosyalarda tutulan appkey’lerin ortak ve dinamik bir yapıyla erişilebilir olmasını ve deployment veya restart, recycle gerektirmeden güncellenmelerini sağlar.

![ConfigurationLibraryDaigram](https://github.com/sertunc/BeymenGroupBackendDeveloperCaseStudy/assets/6024003/e15be2d5-f6d5-4889-bc2b-bde1802461ca)

![ConfigurationLibraryScreenShot1](https://github.com/sertunc/BeymenGroupBackendDeveloperCaseStudy/assets/6024003/079a8a8e-94ae-40fb-9801-2ff2cd241870)

### Backend Servislerini Çalıştırma

Backend servislerini ayağa kaldırmak için:

1. Proje root klasörüne gidin:
    ```bash
    cd /BeymenGroupBackendDeveloperCaseStudy
    ```
2. Docker Compose kullanarak tüm servisleri başlatın:
    ```bash
    docker-compose up --build -d
    ```

### UI Projesini Çalıştırma

UI projesini başlatmak için:

1. UI projesinin bulunduğu dizine gidin:
    ```bash
    cd /BeymenGroupBackendDeveloperCaseStudy/src/Clients/WebApp
    ```
2. Docker image'ini oluşturun:
    ```bash
    docker build . -t "configuration-ui"
    ```
3. Docker container'ını çalıştırın:
    ```bash
    docker run -d --name=configuration-ui -p 3000:3000 configuration-ui
    ```
4. Login Bilgileri:
    ```bash
    Username: 1 Password: 1
    ```

## Servis URL'leri

| Servis Adı                | URL                                |
|---------------------------|------------------------------------|
| Configuration.UI          | [http://localhost:3000](http://localhost:3000)  |
| Configuration.WebApi      | [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)  |
| SERVICE-A                 | [http://localhost:5010/swagger/index.html](http://localhost:5010/swagger/index.html)  |
| SERVICE-B                 | [http://localhost:5020/swagger/index.html](http://localhost:5020/swagger/index.html)  |
| SERVICE-C                 | [http://localhost:5030/swagger/index.html](http://localhost:5030/swagger/index.html)  |

## Kullanım

Servisleri başarıyla çalıştırdıktan sonra, yukarıdaki URL'leri kullanarak ilgili servislerin arayüzlerine ve API'larına erişebilirsiniz.
Config değeri okumak için SERVICE-A,B,C "/api/Configuration" endpointlerine swagger ile istek atabilirsiniz.
