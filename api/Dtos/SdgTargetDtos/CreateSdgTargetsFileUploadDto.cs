using System.ComponentModel.DataAnnotations; 

namespace api.Dtos.SdgTargetDtos
{
  public class CreateSdgTargetFileUploadDto
  {
    public int CompanyId {get; set; }
    public List<CreateSdgTargetDto> CreateSdgTargetDtos { get; set; }
  }
}