using SelectelSharp.Common;
using SelectelSharp.Headers;
using SelectelSharp.Models.Container;
using SelectelSharp.Models.File;
using SelectelSharp.Requests.Container;
using SelectelSharp.Requests.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectelSharp
{
    public static class SelectelClientExtensions
    {
        #region Container

        /// <summary>
        /// Получиение информации по контейнеру
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        public static Task<ContainerInfo> GetContainerInfoAsync(this SelectelClient client, string container)
        {
            return client.ExecuteAsync(new GetContainerInfoRequest(container));
        }

        /// <summary>
        /// Получение списка контейнеров
        /// </summary>
        /// <param name="limit">Число, ограничивает количество объектов в результате (по умолчанию 10000)</param>
        /// <param name="marker">Cтрока, результат будет содержать объекты по значению больше указанного маркера (полезно использовать для постраничной навигации и при большом количестве контейнеров)</param>
        public static Task<ContainersList> GetContainersListAsync(this SelectelClient client, int limit = 1000, string marker = null)
        {
            return client.ExecuteAsync(new GetContainersRequest(limit, marker));
        }

        /// <summary>
        /// Создание нового контейнера
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        /// <param name="customHeaders">Произвольные мета-данные через передачу заголовков с префиксом X-Container-Meta-.</param>
        /// <param name="type">X-Container-Meta-Type: Тип контейнера (public, private, gallery)</param>
        /// <param name="corsHeaders">Дополнительные заголовки кэшировани и CORS</param>
        public static Task<CreateContainerResult> CreateContainerAsync(this SelectelClient client, string container, ContainerType type = ContainerType.Private, Dictionary<string, object> customHeaders = null, CORSHeaders corsHeaders = null)
        {
            return client.ExecuteAsync(new CreateContainerRequest(container, type, customHeaders, corsHeaders));
        }

        /// <summary>
        /// Удаление контейнера
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        public static Task<DeleteContainerResult> DeleteContainerAsync(this SelectelClient client, string container)
        {
            return client.ExecuteAsync(new DeleteContainerRequest(container));
        }

        /// <summary>
        /// Обновление мета-данных контейнера
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        /// <param name="type">X-Container-Meta-Type: Тип контейнера (public, private, gallery)</param>
        /// <param name="customHeaders">Произвольные мета-данные через передачу заголовков с префиксом X-Container-Meta-.</param>
        /// <param name="corsHeaders">Дополнительные заголовки кэшировани и CORS</param>
        public static Task<UpdateContainerResult> SetContainerMetaAsync(this SelectelClient client, string container, ContainerType type = ContainerType.Private, Dictionary<string, object> customHeaders = null, CORSHeaders corsHeaders = null)
        {
            return client.ExecuteAsync(new UpdateContainerMetaRequest(container, type, customHeaders, corsHeaders));
        }

        /// <summary>
        /// Преобразование контейнера в галерею
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        /// <param name="password">Дополнительно можно установить пароль, по которому будет ограничен доступ</param>
        public static Task<UpdateContainerResult> SetContainerToGalleryAsync(this SelectelClient client, string container, string password = null)
        {
            return client.ExecuteAsync(new UpdateContainerToGalleryRequest(container, password));
        }

        /// <summary>
        /// Получение списка файлов в контейнере
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        /// <param name="limit">Число, ограничивает количество объектов в результате (по умолчанию 10000)</param>
        /// <param name="marker">Cтрока, результат будет содержать объекты по значению больше указанного маркера (полезно использовать для постраничной навигации и при большом количестве контейнеров)</param>        
        /// <param name="prefix">Строка, вернуть объекты имена которых начинаются с указанного префикса</param>
        /// <param name="path">Строка, вернуть объекты в указанной папке(виртуальные папки)</param>
        /// <param name="delimeter">Символ, вернуть объекты до указанного разделителя в их имени</param>
        public static Task<ContainerFilesList> GetContainerFilesAsync(this SelectelClient client, string container, int limit = 10000, string marker = null, string prefix = null, string path = null, string delimeter = null)
        {
            return client.ExecuteAsync(new GetContainerFilesRequest(container, limit, marker, prefix, path, delimeter));
        }

        #endregion

        #region Files

        /// <summary>
        /// Получение файла
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        /// <param name="path">Путь к файлу в контейнере</param>
        /// <param name="conditionalHeaders">Условные заголовки GET-запроса</param>
        /// <param name="allowAnonymously">Для файлов в публичных контейнерах, скачиваемых без токена</param>
        /// <returns></returns>
        public static Task<GetFileResult> GetFileAsync(this SelectelClient client, string container, string path, ConditionalHeaders conditionalHeaders = null, bool allowAnonymously = false)
        {
            return client.ExecuteAsync(new GetFileRequest(container, path, conditionalHeaders, allowAnonymously));
        }

        /// <summary>
        /// Сохранение файла
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        /// <param name="pathToSave">Путь к файлу в контейнере</param>
        /// <param name="file">Сохрянемый файл</param>
        /// <param name="validateChecksum">Проверить хэн после загрузки</param>
        /// <param name="contentDisposition">Content-Disposition</param>
        /// <param name="contentType">Content-Type</param>
        /// <param name="deleteAt">Время удаления</param>
        /// <param name="deleteAfter">Промежуток времени до удаления в секундах</param>
        /// <param name="customHeaders">Заголовки файла</param>
        /// <returns></returns>
        public static Task<UploadFileResult> UploadFileAsync(this SelectelClient client, 
            string container, 
            string pathToSave, 
            byte[] file, 
            bool validateChecksum = false,
            string contentDisposition = null,
            string contentType = null,
            DateTime? deleteAt = null,
            long? deleteAfter = null,
            IDictionary<string, object> customHeaders = null)
        {
            return client.ExecuteAsync(new UploadFileRequest(
                container, 
                pathToSave, 
                file, 
                validateChecksum, 
                contentDisposition, 
                contentType,
                deleteAt.HasValue ? Helpers.DateToUnixTimestamp(deleteAt.Value) : (long?)null,
                deleteAfter));
        }

        /// <summary>
        /// Сохранение файла
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        /// <param name="pathToSave">Путь к файлу в контейнере</param>
        /// <param name="localFilePath">Локальный путь к сохраняемому файлу</param>
        /// <param name="validateChecksum">Проверить хэн после загрузки</param>
        /// <param name="contentDisposition">Content-Disposition</param>
        /// <param name="contentType">Content-Type</param>
        /// <param name="deleteAt">Время удаления</param>
        /// <param name="deleteAfter">Промежуток времени до удаления в секундах</param>
        /// <param name="customHeaders">Заголовки файла</param>
        public static Task<UploadFileResult> UploadFileAsync(this SelectelClient client,
           string container,
           string pathToSave,
           string localFilePath,
           bool validateChecksum = false,
           string contentDisposition = null,
           string contentType = null,
           DateTime? deleteAt = null,
           long? deleteAfter = null,
           IDictionary<string, object> customHeaders = null)
        {
            var file = File.ReadAllBytes(localFilePath);
            return client.UploadFileAsync(container, pathToSave, file, validateChecksum, contentDisposition, contentType, deleteAt, deleteAfter);
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        /// <param name="path">Путь к файлу в контейнере</param>
        /// <returns></returns>
        public static Task<DeleteFileResult> DeleteFileAsync(this SelectelClient client, string container, string path)
        {
            return client.ExecuteAsync(new DeleteFileRequest(container, path));
        }

        /// <summary>
        /// Обновление мета-данных файла
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        /// <param name="path">Путь к файлу в контейнере</param>
        /// <param name="customHeaders">Произвольные мета-данные через передачу заголовков с префиксом X-Container-Meta-.</param>
        /// <param name="corsHeaders">Дополнительные заголовки кэшировани и CORS</param>
        public static Task<UpdateFileResult> SetFileMetaAsync(this SelectelClient client, 
            string container, 
            string path, 
            IDictionary<string, object> customHeaders = null,            
            CORSHeaders corsHeaders = null)
        {
            return client.ExecuteAsync(new UpdateFileMetaRequest(container, path, customHeaders, corsHeaders));
        }

        /// <summary>
        /// Загрузка архива с последующей распаковкой на сервере
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        /// <param name="file">Архив</param>
        /// <param name="format">Формат архива</param>
        /// <param name="pathToSave">Путь для распаковки в контейнере</param>
        /// <returns></returns>
        public static Task<UploadArchiveResult> UploadArchiveAsync(this SelectelClient client,
            string container,
            byte[] file,
            FileArchiveFormat format,
            string pathToSave = null)
        {
            return client.ExecuteAsync(new UploadArchiveRequest(file, format, pathToSave));
        }

        /// <summary>
        /// Загрузка архива с последующей распаковкой на сервере
        /// </summary>
        /// <param name="container">Имя контейнера</param>
        /// <param name="localPathToArchive">Локальный путь к загружаемому архиву</param>        
        /// <param name="pathToSave">Путь для распаковки в контейнере</param>
        /// <param name="format">Формат архива</param>
        public static Task<UploadArchiveResult> UploadArchiveAsync(this SelectelClient client,
            string container,
            string localPathToArchive,
            string pathToSave = null,
            FileArchiveFormat? format = null)
        {
            if (!format.HasValue)
            {
                if (localPathToArchive.EndsWith(".tar"))
                {
                    format = FileArchiveFormat.Tar;
                }
                else if (localPathToArchive.EndsWith(".tar.gz"))
                {
                    format = FileArchiveFormat.TarGz;
                }
                else if (localPathToArchive.EndsWith(".tar.bz2"))
                {
                    format = FileArchiveFormat.TarBz2;
                }
            }

            if (!format.HasValue)
            {
                throw new ArgumentException("Not supported archive format");
            }

            var file = File.ReadAllBytes(localPathToArchive);
            return client.UploadArchiveAsync(container, file, format.Value, pathToSave);
        }


        #endregion
    }
}
