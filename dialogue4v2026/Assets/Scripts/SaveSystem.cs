using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private const int TOTAL_SLOTS = 4;

    private string dataPath;

    [SerializeField]
    private List<Save> Saves = new List<Save>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            dataPath = Path.Combine(
                Application.persistentDataPath,
                "save"
            );

            InicializarSlots();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InicializarSlots()
    {
        Saves.Clear();

        for (int i = 0; i < TOTAL_SLOTS; i++)
        {
            Saves.Add(new Save());
        }
    }

    private bool SlotValido(int slot)
    {
        return slot >= 0 && slot < TOTAL_SLOTS;
    }

    // =========================
    // PLAYER LEVEL
    // =========================

    public bool SavePlayerLevel(int level, int slot = 0)
    {
        if (!SlotValido(slot))
            return false;

        Saves[slot].playerLevel = level;
        return true;
    }

    public bool LoadPlayerLevel(out int level, int slot = 0)
    {
        if (!SlotValido(slot))
        {
            level = -1;
            return false;
        }

        level = Saves[slot].playerLevel;
        return true;
    }

    // =========================
    // PLAYER NAME
    // =========================

    public bool SavePlayerName(string playerName, int slot = 0)
    {
        if (!SlotValido(slot))
            return false;

        Saves[slot].playerName = playerName;
        return true;
    }

    public bool LoadPlayerName(out string playerName, int slot = 0)
    {
        if (!SlotValido(slot))
        {
            playerName = "";
            return false;
        }

        playerName = Saves[slot].playerName;
        return true;
    }

    // =========================
    // FASE
    // =========================

    public bool SaveFase(int fase, int slot = 0)
    {
        if (!SlotValido(slot))
            return false;

        Saves[slot].fase = fase;
        return true;
    }

    public bool LoadFase(out int fase, int slot = 0)
    {
        if (!SlotValido(slot))
        {
            fase = -1;
            return false;
        }

        fase = Saves[slot].fase;
        return true;
    }

    // =========================
    // CHECKPOINT
    // =========================

    public bool SaveCheckpoint(
        bool checkpointAtivado,
        int slot = 0)
    {
        if (!SlotValido(slot))
            return false;

        Saves[slot].checkpointAtivado =
            checkpointAtivado;

        return true;
    }

    public bool LoadCheckpoint(
        out bool checkpointAtivado,
        int slot = 0)
    {
        if (!SlotValido(slot))
        {
            checkpointAtivado = false;
            return false;
        }

        checkpointAtivado =
            Saves[slot].checkpointAtivado;

        return true;
    }

    // =========================
    // SALVAR CHECKPOINT COMPLETO
    // =========================

    public bool SalvarCheckpoint(
        int fase,
        Vector3 posicao,
        int moedas,
        List<int> moedasColetadas,
        int slot = 0)
    {
        if (!SlotValido(slot))
            return false;

        Saves[slot].fase = fase;

        Saves[slot].checkpointAtivado = true;

        Saves[slot].checkpointX = posicao.x;
        Saves[slot].checkpointY = posicao.y;
        Saves[slot].checkpointZ = posicao.z;

        Saves[slot].moedasNoCheckpoint = moedas;

        if (moedasColetadas != null)
        {
            Saves[slot].moedasColetadas =
                new List<int>(moedasColetadas);
        }
        else
        {
            Saves[slot].moedasColetadas =
                new List<int>();
        }

        return SaveToFile(slot);
    }

    // =========================
    // POSIÇÃO DO CHECKPOINT
    // =========================

    public bool LoadPosicaoCheckpoint(
        out Vector3 posicao,
        int slot = 0)
    {
        if (!SlotValido(slot))
        {
            posicao = Vector3.zero;
            return false;
        }

        if (!Saves[slot].checkpointAtivado)
        {
            posicao = Vector3.zero;
            return false;
        }

        posicao = new Vector3(
            Saves[slot].checkpointX,
            Saves[slot].checkpointY,
            Saves[slot].checkpointZ
        );

        return true;
    }

    // =========================
    // MOEDAS NO CHECKPOINT
    // =========================

    public bool SaveMoedasNoCheckpoint(
        int quantidade,
        int slot = 0)
    {
        if (!SlotValido(slot))
            return false;

        Saves[slot].moedasNoCheckpoint =
            quantidade;

        return true;
    }

    public bool LoadMoedasNoCheckpoint(
        out int quantidade,
        int slot = 0)
    {
        if (!SlotValido(slot))
        {
            quantidade = 0;
            return false;
        }

        quantidade =
            Saves[slot].moedasNoCheckpoint;

        return true;
    }

    // =========================
    // MOEDAS COLETADAS
    // =========================

    public bool SaveMoedasColetadas(
        List<int> moedas,
        int slot = 0)
    {
        if (!SlotValido(slot))
            return false;

        if (moedas != null)
        {
            Saves[slot].moedasColetadas =
                new List<int>(moedas);
        }
        else
        {
            Saves[slot].moedasColetadas =
                new List<int>();
        }

        return true;
    }

    public bool LoadMoedasColetadas(
        out List<int> moedas,
        int slot = 0)
    {
        if (!SlotValido(slot))
        {
            moedas = new List<int>();
            return false;
        }

        moedas = new List<int>(
            Saves[slot].moedasColetadas
        );

        return true;
    }

    // =========================
    // SALVAR
    // =========================

    public bool SaveToFile(int slot = 0)
    {
        if (!SlotValido(slot))
        {
            Debug.LogError(
                "Slot inválido: " + slot
            );

            return false;
        }

        try
        {
            string json =
                Saves[slot].ToJson();

            string encryptedData =
                Encryptor.Encrypt(json);

            string path =
                dataPath + slot;

            File.WriteAllText(
                path,
                encryptedData
            );

            Debug.Log(
                "Jogo salvo no slot " + slot
            );

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(
                "Erro ao salvar: " + e.Message
            );

            return false;
        }
    }

    // =========================
    // CARREGAR
    // =========================

    public bool LoadFromFile(int slot = 0)
    {
        if (!SlotValido(slot))
            return false;

        string path =
            dataPath + slot;

        if (!File.Exists(path))
        {
            Debug.Log(
                "Slot " + slot + " vazio."
            );

            return false;
        }

        try
        {
            string encryptedData =
                File.ReadAllText(path);

            string json =
                Encryptor.Decrypted(
                    encryptedData
                );

            Saves[slot].FromJson(json);

            Debug.Log(
                "Jogo carregado do slot " + slot
            );

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(
                "Erro ao carregar: " + e.Message
            );

            return false;
        }
    }

    // =========================
    // VERIFICAR SAVE
    // =========================

    public bool SaveExists(int slot)
    {
        if (!SlotValido(slot))
            return false;

        return File.Exists(
            dataPath + slot
        );
    }

    // =========================
    // AUTOSAVE
    // =========================

    public bool AutoSave()
    {
        return SaveToFile(0);
    }

    // =========================
    // SALVAR SLOT MANUAL
    // =========================

    public bool SaveInSlot(int slot)
    {
        if (slot < 1 || slot > 3)
            return false;

        bool saved =
            SaveToFile(slot);

        if (!saved)
            return false;

        CopiarSave(slot, 0);

        return true;
    }

    // =========================
    // CARREGAR SLOT
    // =========================

    public bool LoadFromSlot(int slot)
    {
        if (!SlotValido(slot))
            return false;

        bool loaded =
            LoadFromFile(slot);

        if (!loaded)
            return false;

        CopiarSave(slot, 0);

        return true;
    }

    // =========================
    // COPIAR SAVE
    // =========================

    private void CopiarSave(
        int origem,
        int destino)
    {
        if (!SlotValido(origem) ||
            !SlotValido(destino))
            return;

        string json =
            Saves[origem].ToJson();

        Saves[destino].FromJson(json);

        SaveToFile(destino);
    }

    // =========================
    // PEGAR SAVE
    // =========================

    public Save GetSave(int slot)
    {
        if (!SlotValido(slot))
            return null;

        return Saves[slot];
    }

    // =========================
    // APAGAR SAVE
    // =========================

    public bool DeleteSave(int slot)
    {
        if (!SlotValido(slot))
            return false;

        string path =
            dataPath + slot;

        if (File.Exists(path))
            File.Delete(path);

        Saves[slot] =
            new Save();

        return true;
    }

    // =========================
    // CLASSE SAVE
    // =========================

    [Serializable]
    public class Save
    {
        public int playerLevel;
        public string playerName;

        public int fase;

        public bool checkpointAtivado;

        public float checkpointX;
        public float checkpointY;
        public float checkpointZ;

        public int moedasNoCheckpoint;

        public List<int> moedasColetadas;

        public Save(
            int playerLevel = 0,
            string playerName = "")
        {
            this.playerLevel =
                playerLevel;

            this.playerName =
                playerName;

            fase = 1;

            checkpointAtivado = false;

            checkpointX = 0;
            checkpointY = 0;
            checkpointZ = 0;

            moedasNoCheckpoint = 0;

            moedasColetadas =
                new List<int>();
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void FromJson(string json)
        {
            JsonUtility.FromJsonOverwrite(
                json,
                this
            );

            if (moedasColetadas == null)
            {
                moedasColetadas =
                    new List<int>();
            }
        }
    }

    // =========================
    // CRIPTOGRAFIA
    // =========================

    private class Encryptor
    {
        public static string IV =
            "1a1a1a1a1a1a1a1a";

        public static string Key =
            "1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a13";

        public static string Encrypt(
            string decrypted)
        {
            byte[] textbytes =
                ASCIIEncoding.ASCII.GetBytes(
                    decrypted
                );

            AesCryptoServiceProvider endec =
                new AesCryptoServiceProvider();

            endec.BlockSize = 128;
            endec.KeySize = 256;

            endec.IV =
                ASCIIEncoding.ASCII.GetBytes(IV);

            endec.Key =
                ASCIIEncoding.ASCII.GetBytes(Key);

            endec.Padding =
                PaddingMode.PKCS7;

            endec.Mode =
                CipherMode.CBC;

            ICryptoTransform icrypt =
                endec.CreateEncryptor(
                    endec.Key,
                    endec.IV
                );

            byte[] enc =
                icrypt.TransformFinalBlock(
                    textbytes,
                    0,
                    textbytes.Length
                );

            icrypt.Dispose();
            endec.Dispose();

            return Convert.ToBase64String(enc);
        }

        public static string Decrypted(
            string encrypted)
        {
            byte[] textbytes =
                Convert.FromBase64String(
                    encrypted
                );

            AesCryptoServiceProvider endec =
                new AesCryptoServiceProvider();

            endec.BlockSize = 128;
            endec.KeySize = 256;

            endec.IV =
                ASCIIEncoding.ASCII.GetBytes(IV);

            endec.Key =
                ASCIIEncoding.ASCII.GetBytes(Key);

            endec.Padding =
                PaddingMode.PKCS7;

            endec.Mode =
                CipherMode.CBC;

            ICryptoTransform icrypt =
                endec.CreateDecryptor(
                    endec.Key,
                    endec.IV
                );

            byte[] enc =
                icrypt.TransformFinalBlock(
                    textbytes,
                    0,
                    textbytes.Length
                );

            icrypt.Dispose();
            endec.Dispose();

            return ASCIIEncoding.ASCII.GetString(
                enc
            );
        }
    }
}