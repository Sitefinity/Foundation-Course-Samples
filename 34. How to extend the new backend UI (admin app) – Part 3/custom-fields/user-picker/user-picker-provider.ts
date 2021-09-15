import { Injectable, ClassProvider } from "@angular/core";
import { FIELDS_PROVIDER_TOKEN, FieldData, FieldsProvider, FieldTypes } from "@progress/sitefinity-adminapp-sdk/app/api/v1";
import { FieldRegistration } from "@progress/sitefinity-adminapp-sdk/app/api/v1";
import { RegistrationPair } from "../registration-pair";
import { UserPickerReadonlyComponent } from "./user-picker-readonly.component";
import { UserPickerComponent } from "./user-picker-write.component";
import { SettingsBase } from "@progress/sitefinity-adminapp-sdk/app/api/v1";

/**
 * The fields provider provides the overridden fields back to the AdminApp.
 */
@Injectable()
export class UserPickerProvider implements FieldsProvider {
    private customFieldsMappings: RegistrationPair[];

    constructor() {
        this.customFieldsMappings = [];

        this.registerCustomComponents();
    }

    /**
     * This method gets called before each field is instantiated, allowing custom fields to be plugged in for any type.
     * @param fieldRegistryKey The metadata needed to determine which field to display.
     */
    overrideField(fieldRegistryKey: FieldData): FieldRegistration {
        const registration: FieldRegistration = this.findRegistration(fieldRegistryKey);
        return registration;
    }

    /**
     * This method finds an implementation of the field to be overridden.
     * @param fieldRegistryKey The metadata needed to determine which field to display.
     */
    private findRegistration(fieldRegistryKey: FieldData): FieldRegistration {
        for (const pair of this.customFieldsMappings) {
            if (fieldRegistryKey.fieldName === pair.key.fieldName &&
                fieldRegistryKey.fieldType === pair.key.fieldType &&
                fieldRegistryKey.typeName === pair.key.typeName) {
                    return pair.registration;
            }

            if (pair.key.fieldName === null &&
                pair.key.typeName === null &&
                fieldRegistryKey.fieldType === pair.key.fieldType) {
                return pair.registration;
            }
        }

        return null;
    }

    /**
     * Initializes the custom field(component) registrations.
     */
    private registerCustomComponents(): void {

        // The field name is the name which identifies the field uniquely.
        // The typename is the OData entity set name. It matches the url segment when navigating
        // to the list view of the specific type.
        const customInputKey: FieldData = {
            fieldName: "CustomAuthor",
            fieldType: FieldTypes.shortText,
            typeName: "newsitems"
        };

        // The result field registration that will be returned to the AdminApp.
        const customInputRegistration: FieldRegistration = {
            writeComponent: UserPickerComponent,
            readComponent: UserPickerReadonlyComponent,
            settingsType: SettingsBase
        };

        const customFieldRegistrationPair: RegistrationPair = {
             key: customInputKey,
             registration: customInputRegistration
        };

        this.customFieldsMappings.push(customFieldRegistrationPair);
    }
}

/**
 * Export a 'multi' class provider so that multiple instances of the same provider can coexist.
 * This allows for more than one provider to be registered within one or more bundles.
 */
export const USER_PICKER_PROVIDER: ClassProvider = {
    provide: FIELDS_PROVIDER_TOKEN,
    useClass: UserPickerProvider,
    multi: true
};
