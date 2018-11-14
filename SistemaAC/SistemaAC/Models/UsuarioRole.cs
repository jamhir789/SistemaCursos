using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaAC.Models
{
    public class UsuarioRole
    {
        public List<SelectListItem> usuarioRoles;
        public UsuarioRole()
        {
            usuarioRoles = new List<SelectListItem>();
        }



        //////////////metodo para revisar el rol del usuario
        public async Task<List<SelectListItem>> GetRole(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole>roleManager,string ID)
        {
            usuarioRoles = new List<SelectListItem>();
            string rol;
            var usuario = await userManager.FindByIdAsync(ID);
            var roles = await userManager.GetRolesAsync(usuario);

            if (roles.Count == 0)
            {
                usuarioRoles.Add(new SelectListItem()
                {
                    Value = "null",
                    Text = "No role"
                });
            }
            else
            {
                rol = Convert.ToString(roles[0]);
                var rolesId = roleManager.Roles.Where(m => m.Name == rol);
                // recoremoos data para poder ver el id del role y el nombre 
                foreach (var Data in rolesId)
                {
                    usuarioRoles.Add(new SelectListItem() {
                        Value = Data.Id,
                        Text= Data.Name,

                    });
                }
            }

            return usuarioRoles;

        }
        public List<SelectListItem> Roles(RoleManager<IdentityRole>roleManager) {
            var roles = roleManager.Roles.ToList();
            foreach(var Data in roles)
            {
                usuarioRoles.Add(new SelectListItem() {
                    Value=Data.Id,
                    Text = Data.Name

                });
            }
            return usuarioRoles;
        }



    }
}
