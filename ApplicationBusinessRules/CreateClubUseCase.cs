using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class CreateClubUseCase
    {
        private readonly InsertClub _insertClub;
        private readonly GetStadium _getStadium;

        public CreateClubUseCase(InsertClub insertClub, GetStadium getStadium)
        {
            this._insertClub = insertClub;
            this._getStadium = getStadium;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="club"></param>
        /// <returns>Devuelve el identificar del club insertado</returns>
        public async Task<int> ExecuteAsync(Club club)
        {
            this.EnsureClubIsOkForCreation(club);
            await this.EnsureStadiumExistsAsync(club.StadiumName);
            return await this._insertClub.ExecuteAsync(club);
        }

        private void EnsureClubIsOkForCreation(Club club)
        {
            if (club is null)
            {
                throw new ArgumentException("Club no puede ser nulo");
            }

            if (club.Birthday > DateTime.Now)
            {
                throw new ArgumentException("La fecha de nacimiento no puede ser mayor a la fecha actual");
            }

            if (!club.Email.Contains("@"))
            {
                throw new ArgumentException("El email tiene un formato inválido");
            }
        }

        private async Task EnsureStadiumExistsAsync(string stadiumName)
        {
            Stadium stadium = await this._getStadium.ExecuteAsync(stadiumName);
            if (stadium is null)
            {
                throw new ArgumentException("El estadio no existe");
            }
        }
    }
}
