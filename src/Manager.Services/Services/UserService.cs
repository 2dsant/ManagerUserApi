using EscNet.Hashers.Interfaces.Algorithms;
using EscNet.Cryptography.Interfaces;
using Manager.Services.Interfaces;
using System.Collections.Generic;
using Manager.Infra.Interfaces;
using Manager.Core.Exceptions;
using Manager.Domain.Entities;
using System.Threading.Tasks;
using Manager.Services.DTO;
using AutoMapper;

namespace Manager.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IArgon2IdHasher _hasher;

        public UserService(IMapper mapper, IUserRepository userRepository
            , IArgon2IdHasher hasher)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public async Task<UserDTO> Create(UserDTO userDto)
        {
            var userExists = await _userRepository.GetByEmail(userDto.Email);
            if(userExists != null) throw new DomainException("Já existe um usuário cadastrado com esse email.");
            var user = _mapper.Map<User>(userDto);
            user.Validate();
            user.SetPassword(_hasher.Hash(user.Password));
            var userCreated = await _userRepository.Create(user);
            return _mapper.Map<UserDTO>(userCreated);
        }

        public async Task<UserDTO> Update(UserDTO userDto)
        {
            var userExists = await _userRepository.Get(userDto.Id);
            if(userExists == null) throw new DomainException("Não existe um usuário cadastrado com esse id.");
            var user = _mapper.Map<User>(userDto);
            user.Validate();
            user.SetPassword(_hasher.Hash(user.Password));
            var userCreated = await _userRepository.Update(user);
            return _mapper.Map<UserDTO>(userCreated);
        }

        public async Task Remove(long id)
        {
            await _userRepository.Remove(id);
        }

        public async Task<UserDTO> Get(long id)
        {
            var user = await _userRepository.Get(id);
            return _mapper.Map<UserDTO>(user);        }

        public async Task<List<UserDTO>> Get()
        {
            var allUsers = await _userRepository.Get();
            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        public async Task<UserDTO> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            return _mapper.Map<UserDTO>(user);        }

        public async Task<List<UserDTO>> SearchByEmail(string email)
        {
            var allUsers = await _userRepository.SearchByEmail(email);
            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        public async Task<List<UserDTO>> SearchByName(string name)
        {
            var allUsers = await _userRepository.SearchByName(name);
            return _mapper.Map<List<UserDTO>>(allUsers);        }
    }
}