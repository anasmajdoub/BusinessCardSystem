import { ValidatorFn, AbstractControl, ValidationErrors } from "@angular/forms";

export function addressValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const street = control.get('street');
      const city = control.get('city');
      const state = control.get('state');
      const zipCode = control.get('zipCode');
      const country = control.get('country');
  
      const errors: ValidationErrors = {};
  
      if (street && street.value && street.value.length > 100) {
        errors['streetLength'] = 'Street cannot exceed 100 characters.';
      }
  
      if (city && city.value && city.value.length > 50) {
        errors['cityLength'] = 'City cannot exceed 50 characters.';
      }
  
      if (state && state.value && state.value.length > 50) {
        errors['stateLength'] = 'State cannot exceed 50 characters.';
      }
  
      if (zipCode && zipCode.value && !/^\d{5}(-\d{4})?$/.test(zipCode.value)) {
        errors['zipCodeFormat'] = 'Invalid postal code format.';
      }
  
      if (country && country.value && country.value.length > 50) {
        errors['countryLength'] = 'Country cannot exceed 50 characters.';
      }
  
      return Object.keys(errors).length > 0 ? errors : null;
    };
  }
export function businessCardValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const name = control.get('name');
    const gender = control.get('gender');
    const dateOfBirth = control.get('dateOfBirth');
    const email = control.get('email');
    const phone = control.get('phone');
    const photo = control.get('photo');
    const address = control.get('address');

    const errors: ValidationErrors = {};

    if (name && (!name.value || name.value.trim() === '')) {
      errors['nameRequired'] = 'Name is required.';
    } else if (name && name.value.length > 100) {
      errors['nameLength'] = 'Name cannot exceed 100 characters.';
    }

    if (dateOfBirth && (!dateOfBirth.value || new Date(dateOfBirth.value) > new Date())) {
      errors['dateOfBirthInvalid'] = 'Date of birth cannot be in the future.';
    } else if (dateOfBirth && new Date(dateOfBirth.value) < new Date(new Date().setFullYear(new Date().getFullYear() - 120))) {
      errors['ageExceedsLimit'] = 'Age cannot exceed 120 years.';
    }

    if (email && (!email.value || !/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(email.value))) {
      errors['emailInvalid'] = 'Invalid email format.';
    }

    if (phone && (!phone.value || !/^\+?[1-9]\d{1,14}$/.test(phone.value))) {
      errors['phoneInvalid'] = 'Invalid phone number format.';
    }

    // if (photo && (!photo.value || !isValidBase64(photo.value))) {
    //   errors['photoInvalid'] = 'The provided string is not a valid base64 string.';
    // } else if (photo && !isLessThan1MB(photo.value)) {
    //   errors['photoSizeInvalid'] = 'Image file size must be less than 1 MB.';
    // }

    if (address) {
      const addressErrors = addressValidator()(address);
      if (addressErrors) {
        errors['addressInvalid'] = addressErrors;
      }
    }

    return Object.keys(errors).length > 0 ? errors : null;
  };
}

function isValidBase64(value: string): boolean {
  try {
    return btoa(atob(value)) === value;
  } catch (err) {
    return false;
  }
}

function isLessThan1MB(base64String: string): boolean {
  const stringLength = base64String.length;
  const sizeInBytes = 4 * Math.ceil(stringLength / 3) * 0.5624896334383812;
  const sizeInMB = sizeInBytes / (1024 * 1024);
  return sizeInMB < 1;
}