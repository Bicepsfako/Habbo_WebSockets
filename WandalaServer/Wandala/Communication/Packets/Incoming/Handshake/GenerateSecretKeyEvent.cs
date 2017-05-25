using System;

using Wandala.Communication.Packets.Incoming;
using Wandala.Utilities;
using Wandala.HabboHotel.GameClients;

using Wandala.Communication.Encryption;
using Wandala.Communication.Encryption.Crypto.Prng;
using Wandala.Communication.Packets.Outgoing.Handshake;

namespace Wandala.Communication.Packets.Incoming.Handshake
{
    public class GenerateSecretKeyEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string CipherPublickey = Packet.PopString();
           
            BigInteger SharedKey = HabboEncryptionV2.CalculateDiffieHellmanSharedKey(CipherPublickey);
            if (SharedKey != 0)
            {
                Session.RC4Client = new ARC4(SharedKey.getBytes());
                Session.SendPacket(new SecretKeyComposer(HabboEncryptionV2.GetRsaDiffieHellmanPublicKey()));
            }
            else 
            {
                Session.SendNotification("There was an error logging you in, please try again!");
                return;
            }
        }
    }
}