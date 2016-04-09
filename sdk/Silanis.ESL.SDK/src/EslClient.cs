using System;
using Silanis.ESL.SDK.Internal;
using Silanis.ESL.SDK.Services;
using Silanis.ESL.SDK.Builder;
using Silanis.ESL.API;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Silanis.ESL.SDK
{
    /// <summary>
    /// The EslClient acts as a E-SignLive client.
    /// The EslClient has access to service classes that help create packages and retrieve resources from the client's account.
    /// </summary>
	public class EslClient
	{

		private string baseUrl;
        private string webpageUrl;
		private PackageService packageService;
        private ReportService reportService;
		private SessionService sessionService;
		private FieldSummaryService fieldSummaryService;
		private AuditService auditService;
        private EventNotificationService eventNotificationService;
        private CustomFieldService customFieldService;
        private GroupService groupService;
		private AccountService accountService;
        private ApprovalService approvalService;
		private ReminderService reminderService;
        private TemplateService templateService;
		private AuthenticationTokenService authenticationTokenService;    
		private AttachmentRequirementService attachmentRequirementService;
        private LayoutService layoutService;
        private QRCodeService qrCodeService;
        private AuthenticationService authenticationService;
        private SystemService systemService;
        private SignatureImageService signatureImageService;
        private SigningService signingService;
        
        private JsonSerializerSettings jsonSerializerSettings;

        /// <summary>
        /// EslClient constructor.
        /// Initiates service classes that can be used by the client.
        /// </summary>
        /// <param name="apiKey">The client's api key.</param>
        /// <param name="baseUrl">The staging or production url.</param>
		public EslClient (string apiKey, string baseUrl)
		{
			Asserts.NotEmptyOrNull (apiKey, "apiKey");
			Asserts.NotEmptyOrNull (baseUrl, "baseUrl");
            SetBaseUrl (baseUrl);
            SetWebpageUrl (baseUrl);

            configureJsonSerializationSettings();

            var restClient = new RestClient(apiKey);
            init(restClient, apiKey);
        }

        /// <summary>
        /// EslClient constructor.
        /// Initiates service classes that can be used by the client.
        /// </summary>
        /// <param name="apiKey">The client's api key.</param>
        /// <param name="baseUrl">The staging or production url.</param>
        public EslClient (string apiKey, string baseUrl, string webpageUrl)
        {
            Asserts.NotEmptyOrNull (apiKey, "apiKey");
            Asserts.NotEmptyOrNull (baseUrl, "baseUrl");
            Asserts.NotEmptyOrNull (webpageUrl, "webpageUrl");
            SetBaseUrl (baseUrl);
            this.webpageUrl = AppendServicePath (webpageUrl);

            configureJsonSerializationSettings();

            var restClient = new RestClient(apiKey);
            init(restClient, apiKey);
        }

        public EslClient (string apiKey, string baseUrl, Boolean allowAllSSLCertificates)
        {
            Asserts.NotEmptyOrNull (apiKey, "apiKey");
            Asserts.NotEmptyOrNull (baseUrl, "baseUrl");
            SetBaseUrl (baseUrl);
            SetWebpageUrl (baseUrl);

            configureJsonSerializationSettings();

            var restClient = new RestClient(apiKey, allowAllSSLCertificates);
            init(restClient, apiKey);
        }

        public EslClient (string apiKey, string baseUrl, ProxyConfiguration proxyConfiguration)
        {
            Asserts.NotEmptyOrNull (apiKey, "apiKey");
            Asserts.NotEmptyOrNull (baseUrl, "baseUrl");
            SetBaseUrl (baseUrl);
            SetWebpageUrl (baseUrl);

            configureJsonSerializationSettings();

            var restClient = new RestClient(apiKey, proxyConfiguration);
            init(restClient, apiKey);
        }

        public EslClient (string apiKey, string baseUrl, bool allowAllSSLCertificates, ProxyConfiguration proxyConfiguration)
        {
            Asserts.NotEmptyOrNull (apiKey, "apiKey");
            Asserts.NotEmptyOrNull (baseUrl, "baseUrl");
            SetBaseUrl (baseUrl);
            SetWebpageUrl (baseUrl);

            configureJsonSerializationSettings();

            var restClient = new RestClient(apiKey, allowAllSSLCertificates, proxyConfiguration);
            init(restClient, apiKey);
        }

        private void init(RestClient restClient, String apiKey)
        {
            packageService = new PackageService(restClient, baseUrl, jsonSerializerSettings);
            reportService = new ReportService(restClient, baseUrl, jsonSerializerSettings);
            systemService = new SystemService(restClient, baseUrl, jsonSerializerSettings);
            signingService = new SigningService(restClient, baseUrl, jsonSerializerSettings);
            signatureImageService = new SignatureImageService(restClient, baseUrl, jsonSerializerSettings);
            sessionService = new SessionService(apiKey, baseUrl);
            fieldSummaryService = new FieldSummaryService(new FieldSummaryApiClient(apiKey, baseUrl));
            auditService = new AuditService(apiKey, baseUrl);
            eventNotificationService = new EventNotificationService(new EventNotificationApiClient(restClient, baseUrl, jsonSerializerSettings));
            customFieldService = new CustomFieldService( new CustomFieldApiClient(restClient, baseUrl, jsonSerializerSettings) );
            groupService = new GroupService(new GroupApiClient(restClient, baseUrl, jsonSerializerSettings));
            accountService = new AccountService(new AccountApiClient(restClient, baseUrl, jsonSerializerSettings));
            approvalService = new ApprovalService(new ApprovalApiClient(restClient, baseUrl, jsonSerializerSettings));
            reminderService = new ReminderService(new ReminderApiClient(restClient, baseUrl, jsonSerializerSettings));
            templateService = new TemplateService(new TemplateApiClient(restClient, baseUrl, jsonSerializerSettings), packageService);
            authenticationTokenService = new AuthenticationTokenService(restClient, baseUrl); 
            attachmentRequirementService = new AttachmentRequirementService(restClient, baseUrl, jsonSerializerSettings);
            layoutService = new LayoutService(new LayoutApiClient(restClient, baseUrl, jsonSerializerSettings));
            qrCodeService = new QRCodeService(new QRCodeApiClient(restClient, baseUrl, jsonSerializerSettings));
            authenticationService = new AuthenticationService(webpageUrl);
        }

        private void configureJsonSerializationSettings()
        {
            jsonSerializerSettings = new JsonSerializerSettings ();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            jsonSerializerSettings.Converters.Add( new CultureInfoJsonCreationConverter() );
        }

        private void SetBaseUrl(string baseUrl) 
        {
            this.baseUrl = baseUrl;
            this.baseUrl = AppendServicePath (this.baseUrl);
        }

        private void SetWebpageUrl(string baseUrl) 
        {
            webpageUrl = baseUrl;
            if (webpageUrl.EndsWith("/api")) 
            {
                webpageUrl = webpageUrl.Replace("/api", "");
            }
            webpageUrl = AppendServicePath (webpageUrl);
        }
            
		private string AppendServicePath(string baseUrl)
		{
			if (baseUrl.EndsWith ("/")) 
			{
				baseUrl = baseUrl.Remove (baseUrl.Length - 1);
			}

			return baseUrl;
		}

        /**
         * Facilitates access to the service that could be used to add custom field
         *
         * @return  the custom field service
         */
        public CustomFieldService GetCustomFieldService() {
            return customFieldService;
        }

        internal bool IsSdkVersionSetInPackageData(DocumentPackage package)
        {
            if (package.Attributes != null && package.Attributes.Contents.ContainsKey("sdk"))
            {
                return true;
            }            
            return false;
        }

        internal void SetSdkVersionInPackageData(DocumentPackage package)
        {
            if (package.Attributes == null)
            {
                package.Attributes = new DocumentPackageAttributes();
            }
            package.Attributes.Append( "sdk", ".NET v" + CurrentVersion );
        }

		public PackageId CreatePackage(DocumentPackage package)
        {
            ValidateSignatures(package);
            if (!IsSdkVersionSetInPackageData(package))
            {
                SetSdkVersionInPackageData(package);
            }
        
			var packageToCreate = new DocumentPackageConverter(package).ToAPIPackage();
			var id = packageService.CreatePackage (packageToCreate);
            var retrievedPackage = GetPackage(id);

			foreach (var document in package.Documents)
			{
                UploadDocument(document, retrievedPackage);
			}

			return id;
		}

        public PackageId CreatePackageOneStep(DocumentPackage package)
        {
            ValidateSignatures(package);
            if (!IsSdkVersionSetInPackageData(package))
            {
                SetSdkVersionInPackageData(package);
            }

            var packageToCreate = new DocumentPackageConverter(package).ToAPIPackage();
            foreach(var document in package.Documents){
                packageToCreate.AddDocument(new DocumentConverter(document).ToAPIDocument(packageToCreate));
            }
            var id = packageService.CreatePackageOneStep (packageToCreate, package.Documents);
            return id;
        }

        public void SignDocument(PackageId packageId, string documentName) 
        {
            var package = packageService.GetPackage(packageId);
            foreach(var document in package.Documents) 
            {
                if(document.Name.Equals(documentName)) 
                {
                    document.Approvals.Clear();
                    signingService.SignDocument(packageId, document);
                }
            }
        }

        public void SignDocuments(PackageId packageId) 
        {
            var signedDocuments = new SignedDocuments();
            var package = packageService.GetPackage(packageId);
            foreach(var document in package.Documents) 
            {
                document.Approvals.Clear();
                signedDocuments.AddDocument(document);
            }
            signingService.SignDocuments(packageId, signedDocuments);
        }

        public void SignDocuments(PackageId packageId, string signerId) 
        {
            var bulkSigningKey = "Bulk Signing on behalf of";

            IDictionary<string, string> signerSessionFields = new Dictionary<string, string>();
            signerSessionFields.Add(bulkSigningKey, signerId);
            var signerAuthenticationToken = authenticationTokenService.CreateSignerAuthenticationToken(packageId, signerId, signerSessionFields);

            var signerSessionId = authenticationService.GetSessionIdForSignerAuthenticationToken(signerAuthenticationToken);

            var signedDocuments = new SignedDocuments();
            var package = packageService.GetPackage(packageId);
            foreach(var document in package.Documents) 
            {
                document.Approvals.Clear();
                signedDocuments.AddDocument(document);
            }
            signingService.SignDocuments(packageId, signedDocuments, signerSessionId);
        }

		public PackageId CreateAndSendPackage( DocumentPackage package ) 
		{
			var packageId = CreatePackage (package);
			SendPackage (packageId);
			return packageId;
		}

		public void SendPackage (PackageId id)
		{
			packageService.SendPackage (id);
		}

        public PackageId CreateTemplateFromPackage(PackageId originalPackageId, DocumentPackage delta)
        {
			return templateService.CreateTemplateFromPackage( originalPackageId, new DocumentPackageConverter(delta).ToAPIPackage() );
        }

        public PackageId CreateTemplateFromPackage(PackageId originalPackageId, string templateName)
        {
            var sdkPackage = PackageBuilder.NewPackageNamed( templateName ).Build();
            return CreateTemplateFromPackage( originalPackageId, sdkPackage );
        }
        
        public PackageId CreatePackageFromTemplate(PackageId templateId, string packageName)
        {
            var sdkPackage = PackageBuilder.NewPackageNamed( packageName ).Build();
            return CreatePackageFromTemplate( templateId, sdkPackage );
        }
        
        public PackageId CreatePackageFromTemplate(PackageId templateId, DocumentPackage delta)
        {
            ValidateSignatures(delta);
            SetNewSignersIndexIfRoleWorkflowEnabled(templateId, delta);
			return templateService.CreatePackageFromTemplate( templateId, new DocumentPackageConverter(delta).ToAPIPackage() );
        }

        private void SetNewSignersIndexIfRoleWorkflowEnabled (PackageId templateId, DocumentPackage documentPackage) 
        {
            var template = new DocumentPackageConverter(packageService.GetPackage(templateId)).ToSDKPackage();
            if (CheckSignerOrdering(template)) {
                var firstSignerIndex = template.Signers.Count;
                foreach(var signer in documentPackage.Signers)
                {
                    signer.SigningOrder = firstSignerIndex;
                    firstSignerIndex++;
                }
            }
        }

        private void ValidateSignatures(DocumentPackage documentPackage) {
            foreach(var document in documentPackage.Documents) {
                ValidateMixingSignatureAndAcceptance(document);
            }
        }

        private void ValidateMixingSignatureAndAcceptance(Document document) {
            if(CheckAcceptanceSignatureStyle(document)) {
                foreach(var signature in document.Signatures) {
                    if (signature.Style != SignatureStyle.ACCEPTANCE )
                        throw new EslException("It is not allowed to use acceptance signature styles and other signature styles together in one document.", null);
                }
            }
        }

        private bool CheckAcceptanceSignatureStyle(Document document) {
            foreach (var signature in document.Signatures) {
                if (signature.Style == SignatureStyle.ACCEPTANCE)
                    return true;
            }
            return false;
        }

        private bool CheckSignerOrdering(DocumentPackage template) {
            foreach(var signer in template.Signers)
            {
                if (signer.SigningOrder > 0) 
                {
                    return true;
                }
            }
            return false;
        }

		public PackageId CreateTemplate(DocumentPackage template)
		{
			var templateId = templateService.CreateTemplate(new DocumentPackageConverter(template).ToAPIPackage());
			var createdTemplate = GetPackage(templateId);

			foreach (var document in template.Documents)
			{
				UploadDocument(document, createdTemplate);
			}

			return templateId;
		}

		[Obsolete("Call AuthenticationTokenService.CreateSenderAuthenticationToken() instead.")]
		public SessionToken CreateSenderSessionToken()
		{
			return sessionService.CreateSenderSessionToken();
		}

		[Obsolete("Call AuthenticationTokenService.CreateSignerAuthenticationToken() instead.")]
		public SessionToken CreateSessionToken(PackageId packageId, string signerId)
		{
			return CreateSignerSessionToken(packageId, signerId); 
		}

		public SessionToken CreateSignerSessionToken(PackageId packageId, string signerId)
		{
			return sessionService.CreateSignerSessionToken (packageId, signerId);
		}

        //use createUserAuthenticationToken which returns a string for the token
        [Obsolete("Call AuthenticationTokenService.CreateUserAuthenticationToken() instead.")]
		public AuthenticationToken CreateAuthenticationToken()
		{
			return authenticationTokenService.CreateAuthenticationToken();
		}

        public byte[] DownloadDocument (PackageId packageId, string documentId)
		{
			return packageService.DownloadDocument (packageId, documentId);
		}

        public byte[] DownloadOriginalDocument(PackageId packageId, string documentId)
        {
            return packageService.DownloadOriginalDocument(packageId, documentId);
        }

        public byte[] DownloadEvidenceSummary (PackageId packageId)
		{
			return packageService.DownloadEvidenceSummary (packageId);
		}

        public byte[] DownloadZippedDocuments (PackageId packageId)
		{
			return packageService.DownloadZippedDocuments (packageId);
		}

		public DocumentPackage GetPackage (PackageId id)
		{
			var package = packageService.GetPackage (id);

            return new DocumentPackageConverter(package).ToSDKPackage();
		}

        public void UpdatePackage(PackageId packageId, DocumentPackage sentSettings)
        {
			packageService.UpdatePackage( packageId, new DocumentPackageConverter(sentSettings).ToAPIPackage() );
        }

        public void ChangePackageStatusToDraft(PackageId packageId) {
            packageService.ChangePackageStatusToDraft(packageId);
        }
        
		public SigningStatus GetSigningStatus (PackageId packageId, string signerId, string documentId)
		{
			return packageService.GetSigningStatus (packageId, signerId, documentId);
		}

		public Document UploadDocument(Document document, DocumentPackage documentPackage ) {
			return UploadDocument( document.FileName, document.Content, document, documentPackage );
		}

		public Document UploadDocument(String fileName, byte[] fileContent, Document document, DocumentPackage documentPackage)
        {
			var uploaded = packageService.UploadDocument(documentPackage, fileName, fileContent, document);

			documentPackage.Documents.Add(uploaded);
			return uploaded;
        }

		public Document UploadDocument( Document document, PackageId packageId ) {
			var documentPackage = GetPackage(packageId);

			return UploadDocument(document, documentPackage);
		}

        public void UploadAttachment(PackageId packageId, string attachmentId, string filename, byte[] fileBytes, string signerId) {
            var signerSessionFieldKey = "Upload Attachment on behalf of";

            IDictionary<string, string> signerSessionFields = new Dictionary<string, string>();
            signerSessionFields.Add(signerSessionFieldKey, signerId);
            var signerAuthenticationToken = authenticationTokenService.CreateSignerAuthenticationToken(packageId, signerId, signerSessionFields);
            var signerSessionId = authenticationService.GetSessionIdForSignerAuthenticationToken(signerAuthenticationToken);

            attachmentRequirementService.UploadAttachment(packageId, attachmentId, filename, fileBytes, signerSessionId);
        }
        
        /// <summary>
        /// BaseUrl property
        /// </summary>
		public string BaseUrl {
			get {
				return baseUrl;
			}
		}

        /// <summary>
        /// PackageService property
        /// </summary>
		public PackageService PackageService {
			get {
				return packageService;
			}
		}

        public ReportService ReportService {
            get {
                return reportService;
            }
        }

        public SignatureImageService SignatureImageService {
            get {
                return signatureImageService;
            }
        }
		        
        public TemplateService TemplateService
		{
			get
			{
				return templateService;
			}
		}

        /// <summary>
        /// SessionService property
        /// </summary>
		public SessionService SessionService {
			get {
				return sessionService;
			}
		}

        /// <summary>
        /// FieldSummaryService property
        /// </summary>
		public FieldSummaryService FieldSummaryService {
			get {
				return fieldSummaryService;
			}
		}

        /// <summary>
        /// AuditService property
        /// </summary>
		public AuditService AuditService {
			get {
				return auditService;
			}
		}

        public EventNotificationService EventNotificationService
        {
            get
            {
                return eventNotificationService;
            }
        }

        public GroupService GroupService
        {
            get
            {
                return groupService;
            }
        }

		public AccountService AccountService
		{
			get
			{
				return accountService;
			}
		}

        public ApprovalService ApprovalService
        {
            get
            {
                return approvalService;
            }
        }

		public ReminderService ReminderService
		{
			get
			{
				return reminderService;
			}
		}
        
        public AuthenticationTokenService AuthenticationTokenService
        {
            get
            {
                return authenticationTokenService;
            }
        }
        
        public string CurrentVersion
        {
            get
            {
                return VersionUtil.getVersion();
            }
        }   

		public AttachmentRequirementService AttachmentRequirementService
		{
			get
			{
				return attachmentRequirementService;
			}
		}

        public LayoutService LayoutService
        {
            get
            {
                return layoutService;
            }
        }

        public QRCodeService QrCodeService
        {
            get
            {
                return qrCodeService;
            }
        }

        public SystemService SystemService
        {
            get
            {
                return systemService;
            }
        }

        public SigningService SigningService
        {
            get
            {
                return signingService;
            }
        }
	}
}	
