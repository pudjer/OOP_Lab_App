using AT_API.Utilities;
using AT_Domain.DTOs.InDTOs;
using AT_Domain.DTOs.OutDTOs;
using AT_Domain.Models;
using AT_Infrastructure.Facades;
using AT_Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace AT_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticationFacade _authenticationFacade;
        private readonly IBaseModelRepository<User> _userRepository;

        public UsersController(IAuthenticationFacade authenticationFacade,
            IBaseModelRepository<User> userRepository)
        {
            _authenticationFacade = authenticationFacade;
            _userRepository = userRepository;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<User>> Me()
        {
            var current_user = await this.GetUserAsync();
            return Ok(current_user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<SignedUpDTO>> Register(RegisterDTO inDto)
        {
            // i kinda forgot that it can actually do most of the validation
            // even before reaching the controller
            string username = inDto.Username;
            string password = inDto.Password;
            var outDto = await _authenticationFacade.Register(username, password);

            // this could be literally anything from invalid username or password
            // to signing up with a username that already exists
            // so will need to specify a little bit on this later
            if (outDto == null)
            {
                return BadRequest("User with that username already exists.");
            }

            // actually I even doubt whether the facade actually needs to return a DTO
            // and not something else
            return outDto;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoggedInDTO>> Login(LoginDTO inDto)
        {
            string username = inDto.Username;
            string password = inDto.Password;
            var outDto = await _authenticationFacade.Authenticate(username, password);

            if (outDto == null)
            {
                return BadRequest("Invalid login credentials.");
            }

            return outDto;
        }

        [HttpPost("subscribe")]
        [Authorize]
        public async Task<ActionResult> Subscribe()
        {
            var current_user = await this.GetUserAsync();
            if (current_user == null)
            {
                return NotFound();
            }
            if (current_user.IsSubscribed)
            { /*
KKK000000KKK00KKKKKKKKKKKKKKKKKKKKKKK00K00KKKKK0K00KK000OkkkkkOkkkkkkxkkkkOkxdl;..                            ..;lxkkkkkkkkkkkkOOOOOOkOOkkkkkkkxddddddolc:;;;;cc:;,,;,,,,:llc;,'',,,,,;;::::::::::::;;;;
KKK0000KKKKKKKKKKXXXXKXXXXKKKKKKKKKK000000KKKKKK000K0000OkkkkOOOkkkkkkkkkOOkxo:'.                               .'lxkkkkkkkkkkkOOOOOOkkkkkkxxdddoc;:clllc:::::cc:,,,,,,,;cllc;;,,'',,,,;;:::::::::::;;;;
XXKKKKXXXXXXXXXXXXXXXXXXXXXKKKKKKK0K0000000KKKKK00000000OOOOOOOOOkkkkkkkkkOkdc'.                                  'lxkkkkxxkkkkOO00Okkkkxdoc:',;,......',,',:cc:,,,,,,',:llcc::;;,,,,',,;;;;:::::::;;;;:
NNNNNNNNNNNNNNNNNNNNNXXXXXXKKKKKK00000OOOO0KKKK00000000000OOOOOOOkkkkkkkkkkdc;..                                   .:dxkxxxxkkkkOOOOxocc:;...           .. .,,,'''',,,,:lllcccc:;;;,,,,,,,,;;;;::::::;;;
WWWWWWWWWWWNNNNNNNNNNNNXXXXXKKKK0000000OO0KKKKK0000000000OOOOOOOOkkkkkkkkxxol:;'...   .........                     .;oxxxxxxkxxkkxo:'.                        .''''',;lolllccccc::;;,,',,,,,,;;;;;;;;;;
MMWWWWWWWWWWNNNNNNNNNNNXXXXXKKK000000000O00KK00000000000OOOOOOOOOkkkkkkkkxdollc:;,'....';:cc;,....                   .':lddddxxxxdl,.                           .....'cooooollllccc:::;,,,,,,,,,,,;;;;;;
MMWWWWWWWWWWWWWNNNNNNNNXXXXKKKK000000000OOO0OOOOOOO00O0OOOOOOOOOOkkkkxxxdolc:;,,'.......';lolc::;'..                    .,:loolc;'.                                  .:loooddooollcccc::;;;;;,,,,,,,,,,;
MMWWWWWWWWWWWWWWNNNNNNXXXXXXKKK0000OOOOxdoooc:coodkkOOOOOkOkkOkkkkkkkxdolc;,... .        .:oddddol:,'..                    .,,..                                      .:oooodddooollccc::::;;;;,,,,,,,,,
MWWWWWWWWWWWNNWWNNNNNNXXXXXKKK0000OOxlccllol:.....,;:codxkkkkkkkkkxxxoc:,...              'lodddxxdolc,.                                                               .'coooodddddolllcccc:::;;;,,,,,,,
WWWWWWWWWWWNNNWWNNNNNNXXXXXKKK00OOxc,,::codoo:... .. ...';codxxxxxdoc:'..        ...      'coooddxxxxxdl;.                                                               .';looodddddooollllcc:::;;;,,,,
WWWWWWWWWWWNNNNNNNNNNNXXXXKKK00Od:,..;:codxkdl,.         ...';lool;,.....     ......     .;looodddxxxxxxdl,.                                                               .':odddddddddoooooolcc::;;,,,
WWWWWWWWWWNNNNNWNNNNNNXXXXXKK0x:....,;:ldkOkxd:.     .......  .....     .............   .':llooddddxxxxxxdo:.                                                                'ldxddddddddolcc::::c::;;;,
WWWWNNWWNNNNNNNNNNNNNNXXXXKK0d,.  .',;cldkkxxdc.       ...........        ....','''.....';ccllloolodxdxxxxdo:..                                                              .'codddol:;'.... ..:lllc:;;
NNNNNNNNNNNNNNNNNNNNNNXXXXKOo'.  ..,,;cldkkxddo'             .....        ..',;;;,,,,',;ccccllllllloddddddddl;.                                                                ..','..        .:dddolc::
NNNXK0OOkO00KXNNNNNNXXXK0ko;.......'.,:clddoloo:.                 .     ...';::cc::cccclllllllcclllllooodddddl;.                                                                             .:dxxxddolc
NNNNK0OOxdddddddxxxolcc:,...........';:clddl::;;.                       ..,;clloooodddoooloollcccllllllooddddoc,.                                                                           .;oxxkxxxddl
NNNNNXK0K00OOxolc:;'.... ..  .'...;;;;;::clc:,...                     ....'cloddxxxkxxdooooooollcclllllloodoool;.                                                                    ..    'lddxkkkkkxxd
NNNNNNXK00KXXKOkdl:,'.....''.';,,;:;:c:;,;;,,,'......           ...... .. .,coddxxxkkxddddddddolllllllllooooool:.                                                                        .,ldddxkOkkkkxx
XXNNNNNNX0O0KXK0xdlc::::;;::'';,cd:',c;',:c:;,;,,''......        ......... ..:odxxxkkxddddddddoolllllllllooooolc'                                                                       .,coddodkkkkkkkx
XNNNNNNNNNKOO0Okxxollol;:ol;,.',:dd;'cocclcc:,,,'','...''...      .......... .,ldxxkkxxdddddddoolllllooloooolllc,.                                                                      .,:ldxdddxkkkkkk
NNNNNNNWWWWX0xloxxlccll;,lxl:;,;coxxxxkdldxc:c:,..''...',....       .........  'ldxkkkxdddddddddoolllooooooooolc,.                                    .....                             .',:ldxxxdxxkkkk
NNNNNNNWWWWWN0o:clllcloo:cxkolcclldKkoxOdddccoc;;;;;,...''....      .........  .'lxkkkxddddddddddoooooooodooool;.                                     ';'','.                           ..,;;cdxxxxxxxxx
NNNNNNNWWWWWNKl,:loxddxxdxO00kolc:lOx,,looccc:;,;:col,...','....   ..........   .,oxkkxddxxxxdddddooooooodooolc'.                                     ..'.'..           ..              ..,;;:cldxxxdddx
NNNNNNNWWWWWN0o:loodllOkdk0kxOxl:;:cc'.;lc::::;:clooc;'..',,,''..   ....'.....   .cxkkxxdddddddddddooddddddool:.                                                     .,,',,,.          ...,;;:::codxxxdx
NNNNNNNNWWWNXKOolccldooxxxxo;:lcc::;'..,:clcc:cddooll:,'''',;,'...     ..'....   .,dkkkxdddxxddddddddddddddoolc'.                                                    .',',,'.          ..',,,;::::coxxxx
NNNNNNNNWWWNXX0xl:cdkkddddxxc,,:c:;,..;odoooddkO0Okdoc;''',,;;,'..      .......  .'lxxkxxxxxxxdxxxxxdddddxxdolc,.                                                       ..            ..,;,,,,;;:::codxx
XNNNNNNNWWWNXXX0dclolllooooddll:,';:;;dKKOkOO0KXK0kxoc:;,,,,,;:,'......  ...''....'coddddddddddddxxxxxxdxxxdolc:.                                                                      .;::;;,;;;::::cox
XXNNNNNNNWWNXXXX0kxxxdlooodxxkOOxool:oKWWNNXXXXXKOxoc;;,''...,;;,'...... ...',....,cooddddddxxxxxxxxxxxxxxxdoooc,.                                                                    ..,:c:;;;;;:::::cl
XXXNNNNNNNNNXXXXNK0OOkxkkO00OOKNWWKo;;ok0KKXXXK0kdoc:;;;,,'.',;;,'......  ..','...;lodddddddxxkkkxxxxxxxxxxxdddoc;..                                                                 ..,;:ccc:;;;;;:cccc
XXXNNNNNNNNNXXXXNXXX0OO0KXXXXXNWWNKxc;;cdO0KK0kxdool:;;,,'..'::;,,...... ....,,'..;lodddoooodxxkxxxxxxkkkkkkkxxdol:,..                                                              ..';:ccllcc:;,,;:ccc
XXXXXNNNNNNXXXXXXNNNNX0kO0KXNNNNNNK000OOKXXKXK0kxdlcc::;,'.';::,,;'..........',,.':lddddoooooddxxxkkkkkkOOkkkkxxddoc:'.                                                            ..';:::clllllc:;;;:cc
XXXXXNNNNNNXXXXXNNNNNNKkkkO00K000000KXXXXK000K0kxdoc:;;;;;,;cl:,,,'...........''';coddxddddoodddxxkkkkkkOOOOOkkxxxdolc;'..                                                        ..,:cccclooooool::;;:c
XXXXXNNNNNNXXXNNNNNNNNN0xxkkxkkkkO00KXXXXKOxkOkxxdolc;,',:cooc;,:;''.........',;:coddxxxdddoodddddxxkkkkOOOOOOkkxxxdoolc;..                                                     ..',:llllooooooooool:;;:
KXXXXNNNNNNXXXXXXNNNNNN0xxkdoooxOOO0KKXXK0Okxxxxxxdl:;,,;lodl:,:l;','''......;:cloddxxxxdddddddddddxxxxkOOOOOOkkkkxdddolc,..                                                   ..,:cloollloooooooooolc::
KXXXXNNNNNNXXXXXXNNNNNNXOxxxdlllldxkOO00OOkddxxddol:;;coddoc;;clc:;,,,'....,:clodxxxxkkxxdddddxxxxxxxxxxkOOOOOOkkkkxxddool;..                                                ..';clloooolloooddoddoooolc
KXXXXNNNNNNXXXXXXNNNNNNNXOdxxdolcloxkkkxkkxooddxolcclodddo:,;colc::;;,,...'coodxxxkxxkkkxdxxdxxxxxxxxxxxkOOOOOOkkkkkxxxdddoc,...                                           ...,;cllooooooloooddodddddooo
KXXXXXNNNNNXXXXXXNNNNNNNNXOxxkkxdolooooodddddddxxxkOOkoc:;,:ooolc:;;;;,'.';odxxxxxkxkkkkxxxxxxxxxxxxxxxxxOOOOOOOkkkkkxxxdxxdo:,'...........                            ....';:cccllooooooloooddooddddddo
KKXXXXXNNNXXXXXXXNNNNNNNNNXKOkkkkkkkxxddxkkkkkkxxxxxdl:;:codddollc:;;,''',ldxxkxxxxxxxxxxxxxxxxxxxxxxxxxxkOOOOOOOOkkkxxxxxxxxooc::;,,'''''..                     .  ...'',;:cclllllooooooolooddooddddddd
KKXXXXXNNNXXXXXXNNNNNNNNNNXXXK0OkxxxxkkkOOOkkkxoolc:::coxkkxxxxddoc:::,',:oddxxxddxdddddxxxxxxxxxxxxxxxxxkkkOOOOOOOkkkkxxxkkxxddollc::;;,,,'.         .......... ....,;:cccccllllllooooooolooddddddddddd
KKKXXXXXNNXXXXXXNNNNNNNNNNNNNNXXK0Okxxdoollllllcccodxkkkkkkxxxxkxddddoc:;:lddxxxddxxxxddxxxxxxxxxkxxxxxxxkkkkOOOOOOOkkkxxxkkkxxxddolcc::;;;,.         .';;,'......',:cllllclllllllloooooooooodddoddddddd
KKKXXXXXNNXXXXXXXNNNNNNNNNNNNNNNNXXXXK0OOkkxxxxdodxkO0OkkkOOkxkOOOOOOkdcc::lddxxxxxxxxdxxxxxxxxxkkxxxxxxxkkkkkkkkOOOkkkkxxkkkkxxxxxdolcc:;;,.         .,::::c::::clllloollllllllllooooooooooooddoddddddx
KKKXXXXXNNXXXXXXXNNNNNNNNNNNNNNNNNNNNNNXXXKKKK0OOOOOOOOOO0KK000OOOO00kxol:,:oxxxxxxxxxxxxxxxxxxkkkxxxxxxxxkkkkOkkkOOOkkkkkkOkkkkxxxxdoolc:,..         .,:clllloolooooooooolllllllloooooooooooooodddddddx
KKKXXXXXNXXXXXXXXNNNNNNNNNNNNNNNNNNNNNNNNXXXXXXKKKKKK00kxkO0000OOOxxkkdl:;,;lxkkxxxxxxxxxxxxxxxkkkxxxxxxxxkkkkOkkkkOOkkkkkkkkkkkkxxxxxddoc,.         .':ccloooooooooooooooollllllooooooooooolooodddddddx
KKKXXXXXNXXKXXXXXNNNNNNNNNNNNNNNNNNNNNNNNNXXXXXKkdxkkkxoclkO00000Okoc:;,,;;coxkkxxxxxxxxxxxxxddddxxxxxxxxxkkkkOkkkkkkkkkkkkOOkkkkkkxxxxxdl,.        ..;clllooooooooooooooooollollooooolooooooloodddddddd
KKKKXXXXXXKKKXXXXNNNNNNNNNNNNNNNNNNNNNNNNNNXXXXKkllllc::cok00000000kl::;;:loxkkkkxxxxddooollllllllllloodddxkkkkkkkkkkkkkkkkOOkkkkkkkkkkxxo:.        .;clllloooooodooddodoooollooooooooodddooolooodddoddd
KKKKXXXXXXKKKXXXXXNNNNNNNNNNNNNNNNNNNNNNNNNNNXXK0kdlclccokO000000000OkddodxkkkkkkxddollccccccccccccccccclloxkkkkkkkkkkkkkkkOOOkkkkkkkkxxxxo;..    ..;clllllooooddddddddddooooooooooooooodddddooooddodddd
0KKKKXXXXXKKKXXXXXNNNNNNNNNNNNNNNNNNNNNNNNNNNNXXXKKOxxxkO00000000KKKKK000OOOOkkkxollccc::::::::::::::::cccclodxkkkkkkkkkOOOOOOOkkkkkkxxxxxxdo:,,,,;clllllllodddddddddddddddooooooooooooodddddooodddddddd
00KKKKKXXKKKKKKXXXXXXNNNNNNNNNNNNNNNNNWWNNNNNNXXXXNXXKKKKK00000KKKKKKKKKK0Okkkxdocc:::;;;;::cllllllcccc::::clldxkkkkkkkkkOOOOOOOkkkkkkxxxxkkkxdoooooooooooooddddddddddddddddooooooooooodoooddooodddddddd
O00000KKKK00KKKKKKXXNNNNNNNNNNNWNNNNWWWWWNNNNNNNNNNNXXXXKKKKKKKKKKKKKKKKK0OOkxdol::;;;:codxkOOOOOOOkkkxxdlc:::codkkkkkkkkkkOOOOOOkkkkkkkkkkkkkxxxxdddddddddodxddddddddddddddoooooooooooddoddddoodddddddd
OOO0000K0000KKKKKKXXXXXNNNNNNNNNNNNWWWWWWNNNNNNNNNNNNNXXXKKKKKKKKKKKKKKKK0OOkdlc:;;:ldxkOOOOOOOOkkkkkkkkkkxxol::ldxkkkkkkkkOOOOO00OOOOOOOkkkkkkkkkkxxxxxxxxddxxddxxddddddddddoooooooooooddoddddooodddddd
OOOO00000000KKKKKKXXXXXXXNNNNNNWWWWWWWWWWNNNNNNNNNNNNNNXXXKKKKKKKKKKKKKKK0Okxoc:::oxOOOOOOOOOOOOOkkkkkxxxxxxxxdl:clxkkkkkkOOOOO0000OOOOOOOkkkkkkkkkkkkkxxxxxxxxxxxxxddxxxdddddoooooooddddddddddooodddddd
OOOOO00000000KKKKKKXXXXXXNNNNNNNWWWWWWWWWNNNNNNNNNNNNNNNXXXKKKKKKXKKKKKKK0OkdlcclxOOOkkOOOOOO000OOOOOkkkxxxdddxxdlcldkkkOOOOOOO00000OOOOOOOkkkkkkkkkkkkkkxxxxkkkxxxxxxxxxxxdddooooooddddddddddddoodddddd
kOOOOO00000000KKKKKXXXXXXXNNNNNNNWWWWWWWWWWWWWNNWNNNNNNNNXXXXKKXXXXXKKKKKKOxoccokOOkkOOOOO000000OOOOOOOOkkxddodxxdocldkkOOOOOOO00000OOOOOOOkkkkkkkkkkkkkkkkxxkkkkkkkkxxxxxxxdddooooooddddddddddddoddddxx
kkkOOOOOOOO0000KKKKXXXXXXXNNNNNWWWWWWWWWWWWWWWWWWWNNNNNNNNXXXXXXXXXXXXXKKK0xlldkOOkkkOOO00000O000OOOOOOkkkkxdooodxxocldkOOOOOOO00000000OOOOOOkOOOOOkkkkkkkkkkkkOkkkkkkkxxxxxxxddoooodddddddddddddddddxxd
kkkkkOOOOOOO000KKKKXXXXXXXNNNNNWWWWWWWWWWWWWWNNNWWNNNNXXXNNXXXXXXXXXXXXKKK0xldkOkkkkkO0000O00O000OOOOOkkkkkkxdooodxdollxOOOOOOO00000000OOOOOOOOOOOOOkkkkkkkkkkkOkkkkOkkkxxxxxxxdoodddddddddddddddddddddx
kkkkkOOOOOOOO00000KKXXXXXXXNNNNNWWWWWWWWWWWWNNNNNNNNNNNNNNNXXXXXXXXXXXXXXX0dokOOxxxkk000OOOOOOOOOOOOOOOkkkxxxxolloxxdoldk00OOO0000OOOOOOOOOOOOOOOOOkkkOOkkkkkkkOkkOOOOkkkkkkxxxddodddddddddddxxxxddddxxx
kkkkkkkOOOOOOO0000KKKKXXXXXNNNNNWNWWWWWWWWWWNNNNWWNNNNNNNNNNNNNNXXXXXXXXXX0xxOOkxxxkO00OOOOOOOOOOOOOOOOkkkkxxdoccldkxoookO000O000000OOOOOOOO0OOOOOOOOkOOOkkkkkkkOkOOOOOOOOOOkkkxdddddddddddxxxxxxxdddxxx
xxkkkkkOkkkOOOO000KKKKXXXXXNNNNNWWWWWWWWWWWWWNNNNNWNNNNNNNNNNNNNNNNXXXXXXX0xxOOxdddkOOO000OOOOOOOOOOOOOkkkkxdolc:cdkxdoox000000000K00OOOOOOO00OOOOOOOOOOOOkOkkkkOOOOOOOOOOOOkkkkxdddddxxdddxxxxxxxxdxxxx
xxxxkkkkkkkkOOO0000KKKKKXXXNNNNNNWWWWWWWWWWNNNNNNNWWNNNNNNNNNNNNNNNNNNXXXX0kxOOxoodxOOOOOOOOOOOOOOOOOOOkkkxdolc::cdkkxddxO00000000KK0000OOO000OOOOOOOOOOOOOOkkkkOOOOOOOOOOOOkkkkkxddddxxxddxxxxxxxxdxxxx
xxxxxkkkkkkkkOOOO000KKKKKXXXNNNNNWWWWWWWWWNNNNNNNNNWNNNNNNNNNNNNNNNNNNNNNXKkkO0kooooxkOOOOOOOOOOOOOOkkkkxdolcc:::lxkkxdxk00000000KKKK000OOO000OOOOOOOOOOOOOOkkkkOOOOOOOOOOOOOOkkkxxxxxxxxxxxxxxxxxxdddxx
xxxxxxkkkxxkkkkOOO0000KKKKXXXNNNNNNWWWWWWWWNNWWWWNNNNNNNNNNNNNNNNNNNNNNNNNKOkO0OxollldxxkkkkOOOOOkkkxxddolcc::::cdOOkxxxO0KK0000KKKKKK000000000OOOOOOOOOOOOOOkkkkOOOOOOOOOOOOOOOkkxxxxxxxxxxxxxxxxxxddxx
ddxxxxxxxxxxkkkkkO00000KKKXXXXNNNNNWWWWWWWWWWWNNWNNNNNNNNNNNNNNNNNNNNNNNNNX0kO00Odlc:ccloddddxxxdddoollcc:::;;:cdOOOkkkO0KKKK000KKKKKK000000000OOOOOOOOOOkkOOkkkOOOOOOOOOOOOkkOOOkkxxxxxxxxxxxxxxxxxxxxx
dddxxxxxxxxxxkkkkOOO00000KXXXXNNNNWWWWWWWWWWNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNK0OO00Odl:::::cccccccccccc::::;;;;:lxO0OOOkO0KKKKK0000KKKKKK000000000OOOOOOOOkkkOOkkOOOOOOOOOOOOOOOOOOOkkxkkxxxxxxxxxxxxxxxxx
ddddddxxdddxxxxxkkOOOO000KXXXXNNNWWWWWWWWWNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNXK0O00K0koc:::::::::::::;;;;;;;;:ldO000OOOO0KXKKKKK000KKKKKK000000000OOOOOOOOOOOOOOOOkOOOOOOOOOOOOOOOOOkkxxxxxxxxxxxxxxxxxxxx
ooddddddddddxxxxxkkOOOO00KKKXXNNNNNNWWWWWNNNNNWWWNNNNNNNNNNNNNNNNNNNNWWNNNNNNX0O00KK0kolc:;;;;;;;;;;;;;;:cox0KKK00OO0KKXXKKKKK000KKKKK00000KK000OOO0OOOOOOOOOOOkkkOOOOOOOOOOOOOOOOOkkxxkkkxxxxxxxxxxxxxx
ooddddddddddxxxxxxkkkOOO00KKXXXNNNNNWWWWWWNNWWWWWNNNNNNNNNNNNNNNNNNNNWWNNNNNNNXK000KKKK0kxollccccccllloxkO0KKKK0000KXXXXXKKKKKK00KK00KK00000K000OOO00OOOOOOOOOOkkkOOOOOOOOOOOOOOOOOkkkkkkkkkkkkxxxxxxxxx
ooddodddddddxxdddxxkkkOOO0KKKXXXNNNNWWWWWWNWWWWWWNNNNNNNNNNNNWWNNNNNNWNNNNNNNNNXKK0KKKKKKKK0OOkkkkkO00KKKXKKKKK00KKXXXXXXKKKKKK00K000KK00000K000OOOOOOOOOOOOkkkkkkOOOOOOOOOOOOOOOOOkkkxkkkkkkkkxxxxxxxxx
            */    return NoContent();
            }
            current_user.IsSubscribed = true;
            current_user = await _userRepository.UpdateAsync(current_user);
            return Ok(current_user);
        }

        [HttpPost("unsubscribe")]
        [Authorize]
        public async Task<ActionResult> Unsubscribe()
        {
            var user = await this.GetUserAsync();
            if (user == null)
            {
                return NotFound();
            }
            if (!user.IsSubscribed)
            {
                return NoContent();
            }
            user.IsSubscribed = false;
            user = await _userRepository.UpdateAsync(user);
            return Ok(user);
        }
    }
}
