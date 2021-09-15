import { CustomFieldsModule } from "./custom-fields";
import { SitefinityExtensionStore } from "@progress/sitefinity-adminapp-sdk/app/api/v1";

declare var sitefinityExtensionsStore: SitefinityExtensionStore;

sitefinityExtensionsStore.addExtensionModule(CustomFieldsModule);
