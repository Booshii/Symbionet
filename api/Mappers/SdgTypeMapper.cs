using api.Models;
using api.Dtos.SdgTypeDtos;

namespace api.Mappers
{
    public static class SdgTypeMapper
    {
        public static SdgTypeDto ToSdgTypeDto(this SdgType sdgTypeModel)
        {
            return new SdgTypeDto{
                Id = sdgTypeModel.Id,
                Number = sdgTypeModel.Number,
                Name = sdgTypeModel.Name,
                Color = sdgTypeModel.Color,
            };
        }
        public static SimpleSdgTypeDto ToSimpleSdgTypeDto(this SdgType sdgTypeModel ){
            if (sdgTypeModel == null)
                throw new ArgumentNullException(nameof(sdgTypeModel));

            return new SimpleSdgTypeDto
            {
                Id = sdgTypeModel.Id,
                Name = sdgTypeModel.Name,
            };
        }
    }
}