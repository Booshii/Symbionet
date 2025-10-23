export function validateJson(data, type) {
  if (type === "sdg") {
    return validateSdgJson(data);
  } else if (type === "company") {
    return validateCompanyJson(data);
  } else {
    return { isValid: false, error: "Ungültiger Typ. Erlaubt: 'sdg' oder 'company'." };
  }
}

/******************************************/
/************* validate Sdg ***************/
/******************************************/
function validateSdgJson(data) {
  if (!Array.isArray(data)) {
    return { isValid: false, error: "SDG-Daten müssen ein Array sein." };
  }

  for (const entry of data) {
    if (typeof entry.CompanyId !== 'number') {
      return { isValid: false, error: "CompanyId muss eine Zahl sein." };
    }

    if (!Array.isArray(entry.CreateSdgTargetDtos)) {
      return { isValid: false, error: "CreateSdgTargetDtos muss ein Array sein." };
    }

    for (const m of entry.CreateSdgTargetDtos) {
      if (typeof m.Title !== 'string') {
        return { isValid: false, error: "Title muss ein String sein." };
      }
      if (typeof m.Description !== 'string') {
        return { isValid: false, error: "Description muss ein String sein." };
      }
      if (typeof m.IsDone !== 'boolean') {
        return { isValid: false, error: "IsDone muss ein Boolean (true/false) sein." };
      }
      if (!Array.isArray(m.SdgTypeIds)) {
        return { isValid: false, error: "SdgTypeIds muss ein Array sein." };
      }
      if (!m.SdgTypeIds.every(id => typeof id === 'number')) {
        return { isValid: false, error: "Jedes Element in SdgTypeIds muss eine Zahl sein." };
      }
    }
  }

  return { isValid: true, error: null };
}

/******************************************/
/*********** validate Company *************/
/******************************************/
function validateCompanyJson(data) {
  if (!Array.isArray(data)) {
    return { isValid: false, error: "Company-Daten müssen ein Array sein." };
  }

  for (const entry of data) {
    if (typeof entry.name !== 'string') {
      return { isValid: false, error: "Name muss ein String sein." };
    }
    if (typeof entry.street !== 'string') {
      return { isValid: false, error: "Street muss ein String sein." };
    }
    if (typeof entry.streetNumber !== 'string') {
      return { isValid: false, error: "StreetNumber muss ein String sein." };
    }
    if (typeof entry.postalcode !== 'number') {
      return { isValid: false, error: "Postalcode muss eine Zahl sein." };
    }
    if (typeof entry.city !== 'string') {
      return { isValid: false, error: "City muss ein String sein." };
    }
    if (typeof entry.contactPerson !== 'string') {
      return { isValid: false, error: "ContactPerson muss ein String sein." };
    }
    if (typeof entry.contactEmail !== 'string') {
      return { isValid: false, error: "ContactEmail muss ein String sein." };
    }
    if (typeof entry.contactTel !== 'string') {
      return { isValid: false, error: "ContactTel muss ein String sein." };
    }
    if (typeof entry.industryId !== 'number') {
      return { isValid: false, error: "IndustryId muss eine Zahl sein." };
    }
    if (typeof entry.latitude !== 'string') {
      return { isValid: false, error: "Latitude muss ein String sein (kann aber leer sein)." };
    }
    if (typeof entry.longitude !== 'string') {
      return { isValid: false, error: "Longitude muss ein String sein (kann aber leer sein)." };
    }
    if (typeof entry.websiteLink !== 'string') {
      return { isValid: false, error: "WebsiteLink muss ein String sein." };
    }
  }

  return { isValid: true, error: null };
}
