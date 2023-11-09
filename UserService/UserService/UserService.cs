﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;
using UserRepository.UserLogic;

namespace UserService.UserService
{
    public class UserService : IUserService
    {
        private IUserLogic<UserModel> _user;
        public UserService(IUserLogic<UserModel> user)
        {
            _user = user;
        }
        public IEnumerable<UserModel> GetAll()
        {
            return _user.GetAll();
        }
        public UserModel GetUser(int id)
        {
            return _user.Get(id);
        }
        public void Create(UserModel user)
        {
            _user.Create(user);
        }
        public void Update(UserModel user)
        {
            _user.Update(user);
        }
        public void Delete(int id)
        {
            UserModel user = GetUser(id);
            _user.Delete(user);
            _user.SaveChanges();
        }
    }
}
