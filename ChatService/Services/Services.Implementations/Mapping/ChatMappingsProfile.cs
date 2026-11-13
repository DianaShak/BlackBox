using AutoMapper;
using Domain.Entities.Entities;
using Services.Contracts.Chat;

namespace Services.Implementations.Mapping;

public class ChatMappingsProfile : Profile
{
    public ChatMappingsProfile()
    {
        CreateMap<Chat, ChatDto>();

        CreateMap<CreatingChatDto, Chat>()
            .ForMember(d => d.Id, map => map.Ignore())
            .ForMember(d => d.ChatMembers, map => map.Ignore())
            .ForMember(d => d.Messages, map => map.Ignore());

        CreateMap<UpdatingChatDto, Chat>()
            .ForMember(d => d.Id, map => map.Ignore())
            .ForMember(d => d.ChatMembers, map => map.Ignore())
            .ForMember(d => d.Messages, map => map.Ignore());

        CreateMap<UpdatingChatWithMembersDto, Chat>()
            .ForMember(d => d.Id, map => map.Ignore())
            .ForMember(d => d.ChatMembers, map => map.Ignore())
            .ForMember(d => d.Messages, map => map.Ignore());

        CreateMap<ChatInfo, ChatInfoDto>();
    }
}
