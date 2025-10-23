import axiosInstance from "@/axios";

export async function uploadJsonData(jsonData, endpoint) {
  try {
    console.log("📤 JSON-Upload-Daten:", JSON.stringify(jsonData, null, 2));

    const response = await axiosInstance.post(endpoint, jsonData, {
      headers: { "Content-Type": "application/json"}, // teilt dem Server mit dass es sich um eine json Datei handelt
    }); 
    return { success: true, data: response.data}; 
  } catch (error) {
    console.error("Upload-Fehler:", error);
    return { success: false, error: error.message };
  }
}