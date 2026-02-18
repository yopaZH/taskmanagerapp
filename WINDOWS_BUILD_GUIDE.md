# Подробная инструкция по сборке на Windows

Это пошаговое руководство для сборки APK приложения Task Manager на Windows машине.

## Предусловия

Вам потребуется Windows компьютер (10 или 11) с администраторским доступом.

## Этап 1: Установка .NET SDK

### Шаг 1.1: Загрузите .NET 8 SDK

1. Откройте браузер и перейдите на https://dotnet.microsoft.com/download/dotnet/8.0
2. Скачайте **SDK version 8.0.123** (или более новую версию 8.0.x)
   - Выберите: "Windows x64 SDK"
   - Файловое название будет примерно: `dotnet-sdk-8.0.xxx-win-x64.exe`

### Шаг 1.2: Установите SDK

1. Запустите скачанный файл `dotnet-sdk-8.0.xxx-win-x64.exe`
2. Следуйте инструкциям установщика (нажимайте Next → Install)
3. Дождитесь завершения установки (это может занять несколько минут)
4. На последнем экране нажмите "Close"

### Шаг 1.3: Проверьте установку

1. Откройте PowerShell (нажмите Win+X → Windows PowerShell или Windows Terminal)
2. Выполните команду:
   ```powershell
   dotnet --version
   ```
3. Вы должны увидеть версию 8.0.xxx (например "8.0.123")

## Этап 2: Установка Android Development Kit (ADK)

### Шаг 2.1: Загрузите Java Development Kit (JDK)

1. Перейдите на https://www.oracle.com/java/technologies/downloads/#java11
2. Скачайте **Java 11 LTS** для Windows x64
   - Пример ссылки: "jdk-11.x.x_windows-x64_bin.msi"
   
Или используйте OpenJDK:
   - Перейдите на https://adoptium.net/
   - Выберите Java 11 для Windows x64
   - Формат: "OpenJDK11U-jdk_x64_windows_hotspot_11.x.x_x.msi"

### Шаг 2.2: Установите JDK

1. Запустите скачанный файл `.msi`
2. Следуйте инструкциям (выбирайте стандартные параметры)
3. При вопросе о JAVA_HOME выберите "вкл."
4. По завершении нажмите "Close"

### Шаг 2.3: Проверьте JDK

1. Откройте PowerShell
2. Выполните:
   ```powershell
   java -version
   ```
3. Вы должны увидеть информацию о Java 11.x.x

### Шаг 2.4: Загрузите Android SDK

1. Перейдите на https://developer.android.com/studio/releases/cmdline-tools
2. Скачайте **Command-line Tools** для Windows (примерно 10 МБ)
   - Файл: `cmdline-tools-windows-xxx_latest.zip`

### Шаг 2.5: Установите Android SDK

1. Создайте папку `C:\android-sdk`
2. Распакуйте скачанный архив в `C:\android-sdk`
   - Должна получиться структура: `C:\android-sdk\cmdline-tools\...`
3. Откройте PowerShell и выполните:
   ```powershell
   $env:ANDROID_SDK_ROOT = "C:\android-sdk"
   $env:JAVA_HOME = "C:\Program Files\Java\jdk-11.x.x"  # Обновите путь если установили в другое место
   cd C:\android-sdk\cmdline-tools\bin
   .\sdkmanager.bat "platforms;android-34"
   .\sdkmanager.bat "platform-tools"
   .\sdkmanager.bat "build-tools;34.0.0"
   ```

4. Установите переменные окружения навсегда:
   - Нажмите Win+X → Settings (Параметры)
   - Перейдите в "System" → "About"
   - Нажмите "Advanced system settings" (Дополнительно)
   - Нажмите "Environment Variables" (Переменные окружения)
   - Нажмите "New" (Создать)
   - Добавьте две переменные:
     - Имя: `ANDROID_SDK_ROOT`, Значение: `C:\android-sdk`
     - Имя: `JAVA_HOME`, Значение: `C:\Program Files\Java\jdk-11.x.x` (уточните путь)
   - Нажимайте OK→OK→OK

## Этап 3: Установка MAUI Workloads

### Шаг 3.1: Откройте PowerShell

1. Нажмите Win+X
2. Выберите "Windows Terminal" или "Windows PowerShell"
3. Убедитесь, что запущена с администраторскими правами (если нет, перезапустите)

### Шаг 3.2: Установите MAUI workloads

Выполните команду:
```powershell
dotnet workload restore
dotnet workload install maui
dotnet workload install maui-android
```

Ожидайте завершения для каждой команды (это может занять 5-10 минут).

### Шаг 3.3: Проверьте установку

Выполните:
```powershell
dotnet workload list
```

Вы должны увидеть в списке:
- maui
- maui-android

## Этап 4: Настройка проекта

### Шаг 4.1: Клонируйте репозиторий (если еще не клонировали)

Откройте PowerShell и выполните:
```powershell
cd "C:\Projects"  # Или любое другое место на диске
git clone https://github.com/your-username/TaskManagerApp.git
cd TaskManagerApp
```

Или если используете локальный путь (скопируйте всё из Linux):
```powershell
# Используйте любой из способов копирования:
# - Samba/SMB (если есть общая папка)
# - SCP/SFTP
# - USB флешка
# - OneDrive/Google Drive
```

### Шаг 4.2: Проверьте структуру проекта

Убедитесь, что существуют файлы:
- `TaskManagerApp.csproj`
- `MauiProgram.cs`
- `App.xaml` / `App.xaml.cs`
- `AppShell.xaml` / `AppShell.xaml.cs`
- Папки: `Models/`, `Services/`, `ViewModels/`, `View/`, `Resources/`

### Шаг 4.3: Восстановите зависимости

В PowerShell выполните:
```powershell
cd "C:\Projects\TaskManagerApp"  # Путь к проекту
dotnet restore
```

Должно завершиться без ошибок.

## Этап 5: Сборка APK

### Шаг 5.1: Сборка для Android

Выполните команду:
```powershell
cd "C:\Projects\TaskManagerApp"
dotnet publish -f net8.0-android -c Release
```

Это может занять **15-30 минут** в первый раз (загрузка зависимостей, компиляция).

**Процесс:**
- Вы увидите много логов о сборке
- Будут загружаться Android SDK комоненты
- В конце должна быть строка с зелёным галочкой ✓ и надпись "Build succeeded"

### Шаг 5.2: Получение APK

После завершения сборки найдите APK файл:

```powershell
# Найдите APK
Get-ChildItem "C:\Projects\TaskManagerApp\bin\Release\net8.0-android\*.apk" -Recurse
```

APK должен быть в папке:
```
C:\Projects\TaskManagerApp\bin\Release\net8.0-android\
```

Ищите файл примерно с названием:
```
com.yourapp.taskmanagerapp-Signed.apk
# или
TaskManagerApp-Signed.apk
# или другое похожее название
```

## Этап 6: Установка на Android устройство

### Вариант 1: Через USB кабель на Windows (если Android Debug Bridge установлен)

1. Подключите Android телефон USB кабелем к компьютеру
2. На телефоне включите "USB Debug Mode" (Developer Mode):
   - Android 10+: Settings → Developer options → USB debugging
   - Нажмите "Allow" если появится запрос
3. В PowerShell выполните:
   ```powershell
   adb install "C:\Projects\TaskManagerApp\bin\Release\net8.0-android\com.yourapp.taskmanagerapp-Signed.apk"
   ```
4. Ожидайте завершения - должно написать "Success"

### Вариант 2: Через Linux машину (передача файла и установка оттуда)

**На Windows:**
1. Скопируйте APK файл
2. Переместите на Linux машину (используя сетевую папку, Samba, или флешку)

**На Linux:**
```bash
adb install /path/to/com.yourapp.taskmanagerapp-Signed.apk
```

## Этап 7: Проверка установки

1. На Android телефоне откройте Applications / App Drawer
2. Найдите "Task Manager" (или название приложения)
3. Нажмите на него для запуска
4. Приложение должно открыться и вы сможете:
   - Создавать проекты
   - Добавлять задачи
   - Менять статусы и приоритеты
   - Сохранять данные локально

## Troubleshooting (Решение проблем)

### Проблема: "dotnet: command not found" или "dotnet не распознан"
**Решение:** Переустановите .NET SDK и перезагрузитесь

### Проблема: MSB4236 - Microsoft.Maui.Sdk not found
**Решение:**
```powershell
dotnet workload repair
dotnet workload install maui maui-android --force
```

### Проблема: ANDROID_SDK_ROOT не найден
**Решение:** Проверьте переменные окружения:
```powershell
$env:ANDROID_SDK_ROOT
$env:JAVA_HOME
```

Если пусто - установите переменные:
```powershell
$env:ANDROID_SDK_ROOT = "C:\android-sdk"
$env:JAVA_HOME = "C:\Program Files\Java\jdk-11.x.x"
```

### Проблема: "Unable to locate adb"
**Решение:** Скачайте Android Platform Tools:
1. Перейдите на https://developer.android.com/studio/releases/platform-tools
2. Скачайте для Windows
3. Распакуйте в `C:\android-platform-tools`
4. Добавьте в PATH:
   ```powershell
   $env:PATH += ";C:\android-platform-tools"
   ```

## Полезные команды

```powershell
# Проверить всё:
dotnet --version
java -version
adb --version
echo $env:ANDROID_SDK_ROOT
echo $env:JAVA_HOME

# Полная очистка и перестройка:
dotnet clean
dotnet restore
dotnet publish -f net8.0-android -c Release

# Просмотр список установленных Android platforms:
sdkmanager --list

# Проверить подключённые устройства:
adb devices

# Просмотр логов с устройства:
adb logcat
```

## Дополнительная информация

- **Размер проекта:** После полной сборки может занять 5-10 ГБ
- **Время сборки:** Первая сборка 15-30 минут, каждая следующая 2-5 минут
- **Android версии:** Приложение поддерживает Android 5.0+ (API 21+)
- **Размер APK:** Примерно 50-150 МБ в зависимости от оптимизации

## Успешность

Когда вы увидите:
```
✅ Build succeeded
```

И найдёте файл `*-Signed.apk` в папке `bin/Release/net8.0-android/` - сборка прошла успешно!

После этого вы можете установить приложение на Android устройство и пользоваться.
